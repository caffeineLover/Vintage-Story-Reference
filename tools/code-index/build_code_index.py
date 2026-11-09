#!/usr/bin/env python3
"""
Builds the compact SQLite index used to navigate decompiled Vintage Story C# sources.

The index deliberately favors evidence-backed navigation over full program analysis. In addition to declarations and
bounded reference hints, it records literal Vintage Story class-registration calls and resolves their ``typeof``
arguments to an indexed declaration only when one best candidate can be identified.
"""

import argparse
import datetime as dt
import hashlib
import json
import os
import re
import sqlite3
from dataclasses import dataclass
from pathlib import Path


SCHEMA_VERSION = 2
TOOL_VERSION = "1.1.4"
PARSER_MODE = "conservative-regex"

TYPE_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|sealed|abstract|static|partial|readonly|unsafe|new)\s+)*)"
    r"(?P<kind>class|interface|struct|enum|record)\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)(?P<generics>\s*<[^>{};]+>)?"
    r"(?:\s*:\s*(?P<bases>[^{;]+))?"
)
DELEGATE_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|sealed|abstract|static|partial|readonly|unsafe|new)\s+)*)"
    r"delegate\s+(?:ref\s+)?[A-Za-z_][A-Za-z0-9_`.<>,\[\]\?@\s]*?\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)(?:\s*<[^>{};]+>)?\s*\("
)
NAMESPACE_RE = re.compile(r"^\s*namespace\s+(?P<namespace>[A-Za-z_][A-Za-z0-9_.]*)")
ATTRIBUTE_RE = re.compile(r"^\s*\[[^\]]+\]\s*$")
METHOD_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|static|virtual|override|abstract|sealed|extern|unsafe|async|partial|new)\s+)*)"
    r"(?P<return>[A-Za-z_][A-Za-z0-9_`.<>,\[\]\?@\s]*?)\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)\s*"
    r"\((?P<params>[^;{}()]*(?:\([^)]*\)[^;{}()]*)*)\)\s*"
    r"(?P<terminator>where\b|;|\{|=>|$)"
)
CTOR_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|static|extern|unsafe|new)\s+)*)"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)\s*"
    r"\((?P<params>[^;{}()]*(?:\([^)]*\)[^;{}()]*)*)\)\s*"
    r"(?P<terminator>:\s*(?:base|this)\b|;|\{|=>|$)"
)
PROPERTY_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|static|virtual|override|abstract|sealed|unsafe|new|required)\s+)*)"
    r"(?P<type>[A-Za-z_][A-Za-z0-9_`.<>,\[\]\?@\s]*?)\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)\s*"
    r"(?:\{|=>)"
)
FIELD_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|static|readonly|const|volatile|unsafe|new)\s+)*)"
    r"(?P<type>[A-Za-z_][A-Za-z0-9_`.<>,\[\]\?@\s]*?)\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)\s*(?:=|;|,)"
)
EVENT_RE = re.compile(
    r"^\s*(?P<mods>(?:(?:public|protected|internal|private|static|virtual|override|abstract|sealed|new)\s+)*)"
    r"event\s+(?P<type>[A-Za-z_][A-Za-z0-9_`.<>,\[\]\?@\s]*?)\s+"
    r"(?P<name>[A-Za-z_][A-Za-z0-9_`]*)"
)
TOKEN_RE = re.compile(r"\b[A-Za-z_][A-Za-z0-9_`]*\b")
REGISTRATION_METHOD_KINDS = {
    "RegisterBlockClass": "block",
    "RegisterItemClass": "item",
    "RegisterBlockEntityClass": "block_entity",
    "RegisterBlockBehaviorClass": "block_behavior",
    "RegisterCollectibleBehaviorClass": "collectible_behavior",
    "RegisterBlockEntityBehaviorClass": "block_entity_behavior",
    "RegisterEntityBehaviorClass": "entity_behavior",
    "RegisterEntity": "entity",
    "RegisterEntityRendererClass": "entity_renderer",
    "RegisterInventoryClass": "inventory",
}
REGISTRATION_CALL_RE = re.compile(
    r"\b(?P<method>" + "|".join(sorted(REGISTRATION_METHOD_KINDS, key=len, reverse=True)) + r")\s*"
    r"\(\s*\"(?P<key>(?:\\.|[^\"\\])*)\"\s*,\s*typeof\s*\(\s*(?P<type>[^()]+?)\s*\)\s*\)",
    re.MULTILINE,
)
COMMENT_OR_LITERAL_RE = re.compile(
    r'(?P<string>@"(?:[^"]|"")*"|"(?:\\.|[^"\\])*"|\'(?:\\.|[^\'\\])*\')'
    r"|(?P<comment>//[^\r\n]*|/\*.*?\*/)",
    re.DOTALL,
)
ACCESS = {"public", "protected", "internal", "private"}
MEMBER_KEYWORDS = {
    "if",
    "for",
    "foreach",
    "while",
    "switch",
    "catch",
    "using",
    "lock",
    "return",
    "throw",
    "new",
    "typeof",
    "sizeof",
    "checked",
    "unchecked",
}


@dataclass
class SourceRoot:
    path: Path
    kind: str
    name: str


@dataclass
class TypeDecl:
    namespace: str
    name: str
    kind: str
    modifiers: str
    bases: str
    start_line: int
    end_line: int
    start_index: int
    end_index: int
    attributes: str

    @property
    def full_name(self) -> str:
        return f"{self.namespace}.{self.name}" if self.namespace else self.name


@dataclass
class MemberDecl:
    kind: str
    name: str
    signature: str
    return_type: str
    modifiers: str
    start_line: int
    end_line: int
    attributes: str


@dataclass
class RegistrationDecl:
    """Holds one literal registration call and its source evidence before it is written to SQLite."""

    method_name: str
    registration_kind: str
    registration_key: str
    type_name: str
    line: int
    evidence: str


def main() -> int:
    parser = argparse.ArgumentParser(description="Build the Vintage Story Reference C# code SQLite index.")
    parser.add_argument("--version", required=True, help="Vintage Story version folder, e.g. 1.22.3")
    parser.add_argument("--repo-root", default=".", help="Repository root")
    parser.add_argument("--strong-hash", action="store_true", help="Hash source file contents instead of cheap metadata signatures")
    args = parser.parse_args()

    repo_root = Path(args.repo_root).resolve()
    version = args.version
    version_root = repo_root / "versions" / version
    index_dir = version_root / "Index"
    db_path = index_dir / "code.sqlite"
    tmp_db_path = index_dir / "code.sqlite.tmp"
    manifest_path = index_dir / "code.index.json"

    if not version_root.is_dir():
        raise SystemExit(f"Version root not found: {version_root}")

    source_roots = discover_source_roots(version_root, version)
    if not source_roots:
        raise SystemExit(f"No C# source roots found for version {version}")

    index_dir.mkdir(parents=True, exist_ok=True)
    if tmp_db_path.exists():
        tmp_db_path.unlink()

    stats = {
        "source_roots": len(source_roots),
        "source_files": 0,
        "bytes": 0,
        "types": 0,
        "members": 0,
        "references": 0,
        "registrations": 0,
        "registration_types_resolved": 0,
        "parse_errors": 0,
    }
    root_counts: dict[str, int] = {}
    kind_counts: dict[str, int] = {}
    parse_errors: list[dict[str, str]] = []

    conn = sqlite3.connect(tmp_db_path)
    try:
        conn.execute("PRAGMA journal_mode = OFF")
        conn.execute("PRAGMA synchronous = OFF")
        conn.execute("PRAGMA temp_store = MEMORY")
        create_schema(conn)
        insert_source_roots(conn, repo_root, version, source_roots)

        for root in source_roots:
            for path in iter_cs_files(root.path):
                index_source_file(
                    conn=conn,
                    repo_root=repo_root,
                    root=root,
                    path=path,
                    version=version,
                    stats=stats,
                    root_counts=root_counts,
                    kind_counts=kind_counts,
                    parse_errors=parse_errors,
                    strong_hash=args.strong_hash,
                )

        stats["registration_types_resolved"] = resolve_registration_types(conn)
        conn.commit()
        write_meta(conn, version, version_root, source_roots, stats)
        conn.commit()
    finally:
        conn.close()

    validate_db(tmp_db_path)
    if db_path.exists():
        db_path.unlink()
    tmp_db_path.replace(db_path)

    manifest = {
        "schema_version": SCHEMA_VERSION,
        "tool_version": TOOL_VERSION,
        "parser_mode": PARSER_MODE,
        "generated_at_utc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "repo_root": str(repo_root),
        "game_version": version,
        "index_path": str(db_path),
        "strong_hash": bool(args.strong_hash),
        "source_roots": [
            {"name": root.name, "kind": root.kind, "path": str(root.path)}
            for root in source_roots
        ],
        "stats": stats,
        "root_counts": dict(sorted(root_counts.items())),
        "type_kind_counts": dict(sorted(kind_counts.items())),
        "parse_errors_sample": parse_errors[:50],
        "limitations": [
            "Uses conservative regex extraction instead of a full C# parser.",
            "Symbol declarations, signatures, inheritance, and line numbers are indexed.",
            "Registration extraction requires a supported Register* call with a literal key and typeof(...) value.",
            "Dynamic, helper-wrapped, factory-based, and reflection-based registrations are not inferred.",
            "Registered types are linked to declarations only when one best indexed candidate can be identified.",
            "References are token-based hints, not a reliable call graph.",
            "Open source files for exact implementation details.",
        ],
    }
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")

    print(f"Indexed {stats['source_files']} C# files into {db_path}")
    print(
        f"Types: {stats['types']}; members: {stats['members']}; references: {stats['references']}; "
        f"registrations: {stats['registrations']}"
    )
    print(f"Manifest: {manifest_path}")
    return 0


def discover_source_roots(version_root: Path, version: str) -> list[SourceRoot]:
    roots: list[SourceRoot] = []
    api_root = version_root / "API"
    for name in (
        f"VintagestoryAPI_decompiled_{version}",
        f"VintagestoryLib_decompiled_{version}",
        f"VSCreativeMod_decompiled_{version}",
        f"VSEssentials_decompiled_{version}",
        f"VSSurvivalMod_decompiled_{version}",
    ):
        path = api_root / name
        if path.is_dir():
            roots.append(SourceRoot(path=path, kind="api", name=name.replace(f"_decompiled_{version}", "")))

    mods_root = version_root / "mods"
    if mods_root.is_dir():
        for mod_dir in sorted([p for p in mods_root.iterdir() if p.is_dir()]):
            decompiled_dirs = []
            for dirpath, dirnames, filenames in os.walk(mod_dir):
                current = Path(dirpath)
                lower = current.name.lower()
                if "decompiled" in lower and contains_cs_file(current):
                    decompiled_dirs.append(current)
                    dirnames[:] = []
            for path in sorted(decompiled_dirs):
                rel_name = path.relative_to(mods_root).as_posix()
                roots.append(SourceRoot(path=path, kind="mod", name=rel_name))
            if not decompiled_dirs:
                for path in discover_project_source_dirs(mod_dir):
                    rel_name = path.relative_to(mods_root).as_posix()
                    roots.append(SourceRoot(path=path, kind="mod", name=rel_name))
    return roots


def create_schema(conn: sqlite3.Connection) -> None:
    conn.executescript(
        """
        CREATE TABLE index_meta (
            key TEXT PRIMARY KEY,
            value TEXT NOT NULL
        );

        CREATE TABLE source_roots (
            id INTEGER PRIMARY KEY,
            version TEXT NOT NULL,
            name TEXT NOT NULL,
            kind TEXT NOT NULL,
            rel_path TEXT NOT NULL UNIQUE
        );

        CREATE TABLE source_files (
            id INTEGER PRIMARY KEY,
            root_id INTEGER NOT NULL,
            rel_path TEXT NOT NULL UNIQUE,
            namespace_hint TEXT,
            size INTEGER NOT NULL,
            mtime_ns INTEGER NOT NULL,
            signature TEXT NOT NULL,
            parse_status TEXT NOT NULL,
            FOREIGN KEY(root_id) REFERENCES source_roots(id)
        );

        CREATE INDEX idx_source_files_root ON source_files(root_id);
        CREATE INDEX idx_source_files_path ON source_files(rel_path);

        CREATE TABLE types (
            id INTEGER PRIMARY KEY,
            file_id INTEGER NOT NULL,
            root_id INTEGER NOT NULL,
            namespace TEXT,
            name TEXT NOT NULL,
            full_name TEXT NOT NULL,
            kind TEXT NOT NULL,
            accessibility TEXT,
            modifiers TEXT,
            base_type TEXT,
            interfaces TEXT,
            attributes TEXT,
            start_line INTEGER NOT NULL,
            end_line INTEGER NOT NULL,
            FOREIGN KEY(file_id) REFERENCES source_files(id),
            FOREIGN KEY(root_id) REFERENCES source_roots(id)
        );

        CREATE INDEX idx_types_name ON types(name);
        CREATE INDEX idx_types_full_name ON types(full_name);
        CREATE INDEX idx_types_kind ON types(kind);
        CREATE INDEX idx_types_base ON types(base_type);

        CREATE TABLE members (
            id INTEGER PRIMARY KEY,
            type_id INTEGER NOT NULL,
            file_id INTEGER NOT NULL,
            root_id INTEGER NOT NULL,
            kind TEXT NOT NULL,
            name TEXT NOT NULL,
            signature TEXT NOT NULL,
            return_type TEXT,
            accessibility TEXT,
            modifiers TEXT,
            attributes TEXT,
            start_line INTEGER NOT NULL,
            end_line INTEGER NOT NULL,
            FOREIGN KEY(type_id) REFERENCES types(id),
            FOREIGN KEY(file_id) REFERENCES source_files(id),
            FOREIGN KEY(root_id) REFERENCES source_roots(id)
        );

        CREATE INDEX idx_members_name ON members(name);
        CREATE INDEX idx_members_kind ON members(kind);
        CREATE INDEX idx_members_type ON members(type_id);

        CREATE TABLE references_hint (
            id INTEGER PRIMARY KEY,
            file_id INTEGER NOT NULL,
            member_id INTEGER,
            token TEXT NOT NULL,
            line INTEGER NOT NULL,
            FOREIGN KEY(file_id) REFERENCES source_files(id),
            FOREIGN KEY(member_id) REFERENCES members(id)
        );

        CREATE INDEX idx_references_token ON references_hint(token);

        CREATE TABLE registrations (
            id INTEGER PRIMARY KEY,
            file_id INTEGER NOT NULL,
            root_id INTEGER NOT NULL,
            containing_type_id INTEGER,
            member_id INTEGER,
            registered_type_id INTEGER,
            method_name TEXT NOT NULL,
            registration_kind TEXT NOT NULL,
            registration_key TEXT NOT NULL,
            type_name TEXT NOT NULL,
            line INTEGER NOT NULL,
            evidence TEXT NOT NULL,
            FOREIGN KEY(file_id) REFERENCES source_files(id),
            FOREIGN KEY(root_id) REFERENCES source_roots(id),
            FOREIGN KEY(containing_type_id) REFERENCES types(id),
            FOREIGN KEY(member_id) REFERENCES members(id),
            FOREIGN KEY(registered_type_id) REFERENCES types(id)
        );

        CREATE INDEX idx_registrations_key ON registrations(registration_key);
        CREATE INDEX idx_registrations_type ON registrations(type_name);
        CREATE INDEX idx_registrations_kind ON registrations(registration_kind);
        CREATE INDEX idx_registrations_containing_type ON registrations(containing_type_id);
        CREATE INDEX idx_registrations_registered_type ON registrations(registered_type_id);

        CREATE VIRTUAL TABLE code_text USING fts5(
            rel_path,
            root_name,
            namespace,
            symbol_text
        );
        """
    )


def insert_source_roots(conn: sqlite3.Connection, repo_root: Path, version: str, source_roots: list[SourceRoot]) -> None:
    for root in source_roots:
        conn.execute(
            "INSERT INTO source_roots(version, name, kind, rel_path) VALUES (?, ?, ?, ?)",
            (version, root.name, root.kind, root.path.relative_to(repo_root).as_posix()),
        )


def write_meta(conn: sqlite3.Connection, version: str, version_root: Path, source_roots: list[SourceRoot], stats: dict[str, int]) -> None:
    values = {
        "schema_version": str(SCHEMA_VERSION),
        "tool_version": TOOL_VERSION,
        "parser_mode": PARSER_MODE,
        "game_version": version,
        "version_root": str(version_root),
        "generated_at_utc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "source_roots": str(len(source_roots)),
        "source_files": str(stats["source_files"]),
        "types": str(stats["types"]),
        "members": str(stats["members"]),
        "references": str(stats["references"]),
        "registrations": str(stats["registrations"]),
        "registration_types_resolved": str(stats["registration_types_resolved"]),
    }
    conn.executemany("INSERT INTO index_meta(key, value) VALUES (?, ?)", values.items())


def iter_cs_files(root: Path):
    for dirpath, dirnames, filenames in os.walk(root):
        dirnames[:] = sorted(name for name in dirnames if name not in {"bin", "obj", ".vs", ".idea"})
        for filename in sorted(filenames):
            if filename.endswith(".cs"):
                yield Path(dirpath) / filename


def contains_cs_file(root: Path) -> bool:
    for dirpath, dirnames, filenames in os.walk(root):
        dirnames[:] = [name for name in dirnames if name not in {"bin", "obj", ".vs", ".idea"}]
        if any(filename.endswith(".cs") for filename in filenames):
            return True
    return False


def discover_project_source_dirs(mod_dir: Path) -> list[Path]:
    roots: list[Path] = []
    ignored = {"bin", "obj", ".vs", ".idea", "assets"}
    for dirpath, dirnames, filenames in os.walk(mod_dir):
        dirnames[:] = [name for name in dirnames if name not in ignored]
        current = Path(dirpath)
        if any(filename.endswith(".csproj") for filename in filenames) and contains_cs_file(current):
            roots.append(current)
            dirnames[:] = []
    return sorted(roots)


def index_source_file(
    conn: sqlite3.Connection,
    repo_root: Path,
    root: SourceRoot,
    path: Path,
    version: str,
    stats: dict[str, int],
    root_counts: dict[str, int],
    kind_counts: dict[str, int],
    parse_errors: list[dict[str, str]],
    strong_hash: bool,
) -> None:
    root_id = conn.execute(
        "SELECT id FROM source_roots WHERE rel_path = ?",
        (root.path.relative_to(repo_root).as_posix(),),
    ).fetchone()[0]
    rel_path = path.relative_to(repo_root).as_posix()
    stat = path.stat()
    stats["source_files"] += 1
    stats["bytes"] += stat.st_size
    root_counts[root.name] = root_counts.get(root.name, 0) + 1

    try:
        text = path.read_text(encoding="utf-8-sig", errors="replace")
        lines = text.splitlines()
        clean_lines = sanitize_lines(lines)
        namespace_hint = find_namespace_hint(clean_lines)
        declarations = parse_types(lines, clean_lines)
        parse_status = "ok"
    except Exception as exc:
        text = ""
        lines = []
        clean_lines = []
        namespace_hint = None
        declarations = []
        parse_status = f"error: {type(exc).__name__}: {str(exc)[:160]}"
        stats["parse_errors"] += 1
        if len(parse_errors) < 100:
            parse_errors.append({"path": rel_path, "error": parse_status})

    cur = conn.execute(
        """
        INSERT INTO source_files(root_id, rel_path, namespace_hint, size, mtime_ns, signature, parse_status)
        VALUES (?, ?, ?, ?, ?, ?, ?)
        """,
        (
            root_id,
            rel_path,
            namespace_hint,
            stat.st_size,
            stat.st_mtime_ns,
            file_signature(path, stat, strong_hash),
            parse_status,
        ),
    )
    file_id = cur.lastrowid

    symbol_text_parts = [rel_path, root.name, namespace_hint or ""]
    type_locations: list[tuple[int, int, int]] = []
    member_locations: list[tuple[int, int, int]] = []
    for decl in declarations:
        base_type, interfaces = split_bases(decl.bases)
        accessibility = accessibility_from_modifiers(decl.modifiers)
        type_cur = conn.execute(
            """
            INSERT INTO types(file_id, root_id, namespace, name, full_name, kind, accessibility, modifiers,
                              base_type, interfaces, attributes, start_line, end_line)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
            """,
            (
                file_id,
                root_id,
                decl.namespace,
                decl.name,
                decl.full_name,
                decl.kind,
                accessibility,
                decl.modifiers,
                base_type,
                interfaces,
                decl.attributes,
                decl.start_line,
                decl.end_line,
            ),
        )
        type_id = type_cur.lastrowid
        type_locations.append((decl.start_line, decl.end_line, type_id))
        stats["types"] += 1
        kind_counts[decl.kind] = kind_counts.get(decl.kind, 0) + 1
        symbol_text_parts.extend([decl.name, decl.full_name, decl.kind, decl.modifiers, decl.bases, decl.attributes])

        members = parse_members(lines, decl, clean_lines)
        for member in members:
            member_accessibility = accessibility_from_modifiers(member.modifiers)
            member_cur = conn.execute(
                """
                INSERT INTO members(type_id, file_id, root_id, kind, name, signature, return_type, accessibility,
                                    modifiers, attributes, start_line, end_line)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (
                    type_id,
                    file_id,
                    root_id,
                    member.kind,
                    member.name,
                    member.signature,
                    member.return_type,
                    member_accessibility,
                    member.modifiers,
                    member.attributes,
                    member.start_line,
                    member.end_line,
                ),
            )
            member_id = member_cur.lastrowid
            member_locations.append((member.start_line, member.end_line, member_id))
            stats["members"] += 1
            symbol_text_parts.extend(
                [
                    member.name,
                    member.kind,
                    member.signature,
                    member.return_type,
                    member.modifiers,
                    member.attributes,
                ]
            )

            for token, line_no in reference_tokens(clean_lines, member.start_line, member.end_line):
                conn.execute(
                    "INSERT INTO references_hint(file_id, member_id, token, line) VALUES (?, ?, ?, ?)",
                    (file_id, member_id, token, line_no),
                )
                stats["references"] += 1

    # Registration calls are file-level evidence. Associate each call with the narrowest parsed member that contains
    # its line, but retain the call even when conservative member parsing cannot identify its enclosing method.
    for registration in extract_registration_calls(text):
        containing_type_id = find_narrowest_span_id(registration.line, type_locations)
        member_id = find_containing_member_id(registration.line, member_locations)
        conn.execute(
            """
            INSERT INTO registrations(
                file_id, root_id, containing_type_id, member_id, method_name, registration_kind, registration_key,
                type_name, line, evidence
            )
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
            """,
            (
                file_id,
                root_id,
                containing_type_id,
                member_id,
                registration.method_name,
                registration.registration_kind,
                registration.registration_key,
                registration.type_name,
                registration.line,
                registration.evidence,
            ),
        )
        stats["registrations"] += 1
        symbol_text_parts.extend(
            [
                registration.method_name,
                registration.registration_key,
                registration.type_name,
            ]
        )

    conn.execute(
        "INSERT INTO code_text(rowid, rel_path, root_name, namespace, symbol_text) VALUES (?, ?, ?, ?, ?)",
        (file_id, rel_path, root.name, namespace_hint or "", compact_text(symbol_text_parts)),
    )


def find_namespace_hint(lines: list[str]) -> str | None:
    for line in lines[:120]:
        match = NAMESPACE_RE.match(line)
        if match:
            return match.group("namespace")
    return None


def parse_types(lines: list[str], clean_lines: list[str] | None = None) -> list[TypeDecl]:
    if clean_lines is None:
        clean_lines = sanitize_lines(lines)

    declarations: list[TypeDecl] = []
    current_namespace = ""
    for idx, line in enumerate(clean_lines):
        ns_match = NAMESPACE_RE.match(line)
        if ns_match:
            current_namespace = ns_match.group("namespace")

        match = TYPE_RE.match(line)
        delegate_match = DELEGATE_RE.match(line)
        if not match and not delegate_match:
            continue

        if delegate_match:
            kind = "delegate"
            name = clean_symbol_name(delegate_match.group("name"))
            modifiers = normalize_space(delegate_match.group("mods") or "")
            bases = ""
            end_index = find_statement_end(clean_lines, idx)
        else:
            kind = match.group("kind")
            name = clean_symbol_name(match.group("name"))
            modifiers = normalize_space(match.group("mods") or "")
            bases = normalize_space(match.group("bases") or "")
            end_index = find_block_end(clean_lines, idx)
        start_line = idx + 1
        end_line = end_index + 1
        declarations.append(
            TypeDecl(
                namespace=current_namespace,
                name=name,
                kind=kind,
                modifiers=modifiers,
                bases=bases,
                start_line=start_line,
                end_line=end_line,
                start_index=idx,
                end_index=end_index,
                attributes=collect_attributes(lines, idx),
            )
        )
    return declarations


def parse_members(
    lines: list[str],
    decl: TypeDecl,
    clean_lines: list[str] | None = None,
) -> list[MemberDecl]:
    if clean_lines is None:
        clean_lines = sanitize_lines(lines)

    members: list[MemberDecl] = []
    open_index = find_opening_brace(clean_lines, decl.start_index, decl.end_index)
    if open_index is None:
        return members
    start = open_index + 1
    end = decl.end_index
    depth = 1
    parenthesis_depth = 0
    skip_through = -1
    for idx in range(start, end):
        line = clean_lines[idx].strip()
        if idx <= skip_through:
            depth += brace_delta(clean_lines[idx])
            parenthesis_depth = max(0, parenthesis_depth + parenthesis_delta(clean_lines[idx]))
            continue
        if not line:
            depth += brace_delta(clean_lines[idx])
            parenthesis_depth = max(0, parenthesis_depth + parenthesis_delta(clean_lines[idx]))
            continue

        if depth == 1 and parenthesis_depth == 0 and DELEGATE_RE.match(line):
            skip_through = find_statement_end(clean_lines, idx)
        elif depth == 1 and parenthesis_depth == 0 and not ATTRIBUTE_RE.match(line):
            field_match = FIELD_RE.match(line)
            member = parse_member_line(lines, idx, decl, clean_lines)
            if member:
                members.append(member)
                skip_through = max(skip_through, member.end_line - 1)
            elif field_match and "=" in line:
                skip_through = find_statement_end(clean_lines, idx)

        depth += brace_delta(clean_lines[idx])
        parenthesis_depth = max(0, parenthesis_depth + parenthesis_delta(clean_lines[idx]))
        if depth < 1:
            depth = 1
    return dedupe_members(members)


def find_opening_brace(lines: list[str], start_idx: int, end_idx: int) -> int | None:
    for idx in range(start_idx, min(len(lines), end_idx + 1)):
        if "{" in strip_strings_and_comments(lines[idx]):
            return idx
    return None


def parse_member_line(
    lines: list[str],
    idx: int,
    decl: TypeDecl,
    clean_lines: list[str] | None = None,
) -> MemberDecl | None:
    if clean_lines is None:
        clean_lines = sanitize_lines(lines)

    line = normalize_space(clean_lines[idx])
    if not line or line.startswith("#") or any(line.startswith(keyword + " ") for keyword in MEMBER_KEYWORDS):
        return None
    if TYPE_RE.match(line) or DELEGATE_RE.match(line):
        return None

    attributes = collect_attributes(lines, idx)
    end_line = find_member_end(clean_lines, idx)
    signature = normalize_space(strip_comments_preserve_layout(lines[idx]))

    event_match = EVENT_RE.match(line)
    if event_match:
        return MemberDecl(
            kind="event",
            name=clean_symbol_name(event_match.group("name")),
            signature=signature,
            return_type=normalize_space(event_match.group("type")),
            modifiers=normalize_space(event_match.group("mods") or ""),
            start_line=idx + 1,
            end_line=end_line,
            attributes=attributes,
        )

    ctor_match = CTOR_RE.match(line)
    if (
        ctor_match
        and clean_symbol_name(ctor_match.group("name")) == decl.name
        and has_valid_declaration_continuation(
            clean_lines,
            idx,
            ctor_match.group("terminator"),
            allow_constructor_initializer=True,
        )
    ):
        return MemberDecl(
            kind="constructor",
            name=decl.name,
            signature=signature,
            return_type="",
            modifiers=normalize_space(ctor_match.group("mods") or ""),
            start_line=idx + 1,
            end_line=end_line,
            attributes=attributes,
        )

    method_match = METHOD_RE.match(line)
    if method_match and has_valid_declaration_continuation(
        clean_lines,
        idx,
        method_match.group("terminator"),
        allow_constructor_initializer=False,
    ):
        name = clean_symbol_name(method_match.group("name"))
        return_type = normalize_space(method_match.group("return"))
        if name in MEMBER_KEYWORDS:
            return None
        if re.match(r"^from\s+.+\s+in$", return_type):
            return None
        return MemberDecl(
            kind="method",
            name=name,
            signature=signature,
            return_type=return_type,
            modifiers=normalize_space(method_match.group("mods") or ""),
            start_line=idx + 1,
            end_line=end_line,
            attributes=attributes,
        )

    property_match = PROPERTY_RE.match(line)
    if property_match and "(" not in line:
        name = clean_symbol_name(property_match.group("name"))
        if name in MEMBER_KEYWORDS:
            return None
        return MemberDecl(
            kind="property",
            name=name,
            signature=signature,
            return_type=normalize_space(property_match.group("type")),
            modifiers=normalize_space(property_match.group("mods") or ""),
            start_line=idx + 1,
            end_line=end_line,
            attributes=attributes,
        )

    field_match = FIELD_RE.match(line)
    if field_match and "(" not in line:
        name = clean_symbol_name(field_match.group("name"))
        if name in MEMBER_KEYWORDS:
            return None
        return MemberDecl(
            kind="field",
            name=name,
            signature=signature,
            return_type=normalize_space(field_match.group("type")),
            modifiers=normalize_space(field_match.group("mods") or ""),
            start_line=idx + 1,
            end_line=end_line,
            attributes=attributes,
        )

    return None


def has_valid_declaration_continuation(
    lines: list[str],
    idx: int,
    terminator: str,
    *,
    allow_constructor_initializer: bool,
) -> bool:
    if terminator:
        return True

    for candidate in lines[idx + 1 :]:
        clean = normalize_space(strip_strings_and_comments(candidate))
        if not clean:
            continue
        if clean.startswith(("{", "=>", "where ")):
            return True
        if allow_constructor_initializer and re.match(r"^:\s*(?:base|this)\b", clean):
            return True
        return False
    return False


def dedupe_members(members: list[MemberDecl]) -> list[MemberDecl]:
    seen = set()
    result = []
    for member in members:
        key = (member.kind, member.name, member.start_line)
        if key in seen:
            continue
        seen.add(key)
        result.append(member)
    return result


def collect_attributes(lines: list[str], idx: int) -> str:
    attrs = []
    cursor = idx - 1
    while cursor >= 0:
        line = lines[cursor].strip()
        if not line:
            cursor -= 1
            continue
        if ATTRIBUTE_RE.match(line):
            attrs.append(line)
            cursor -= 1
            continue
        break
    attrs.reverse()
    return " ".join(attrs)


def find_block_end(lines: list[str], start_idx: int) -> int:
    depth = 0
    opened = False
    for idx in range(start_idx, len(lines)):
        delta = brace_delta(lines[idx])
        if "{" in strip_strings_and_comments(lines[idx]):
            opened = True
        depth += delta
        if opened and depth <= 0:
            return idx
    return start_idx


def find_statement_end(lines: list[str], start_idx: int) -> int:
    for idx in range(start_idx, min(len(lines), start_idx + 200)):
        if ";" in lines[idx]:
            return idx
    return start_idx


def find_member_end(lines: list[str], start_idx: int) -> int:
    depth = 0
    opened = False
    parenthesis_depth = 0
    for idx in range(start_idx, min(len(lines), start_idx + 400)):
        clean = strip_strings_and_comments(lines[idx])
        for char in clean:
            if opened:
                if char == "{":
                    depth += 1
                elif char == "}":
                    depth -= 1
                    if depth <= 0:
                        return idx + 1
                continue

            if char == "(":
                parenthesis_depth += 1
            elif char == ")":
                parenthesis_depth = max(0, parenthesis_depth - 1)
            elif char == "{" and parenthesis_depth == 0:
                opened = True
                depth = 1
            elif char == ";" and parenthesis_depth == 0:
                return idx + 1
    return start_idx + 1


def brace_delta(line: str) -> int:
    clean = strip_strings_and_comments(line)
    return clean.count("{") - clean.count("}")


def parenthesis_delta(line: str) -> int:
    clean = strip_strings_and_comments(line)
    return clean.count("(") - clean.count(")")


def strip_strings_and_comments(text: str) -> str:
    return COMMENT_OR_LITERAL_RE.sub(lambda match: mask_preserve_layout(match.group(0)), text)


def sanitize_lines(lines: list[str]) -> list[str]:
    return strip_strings_and_comments("\n".join(lines)).split("\n")


def mask_preserve_layout(value: str) -> str:
    return "".join("\n" if char == "\n" else "\r" if char == "\r" else " " for char in value)


def split_bases(bases: str) -> tuple[str, str]:
    if not bases:
        return "", ""
    parts = [normalize_space(part) for part in split_comma_safe(bases)]
    if not parts:
        return "", ""
    first = parts[0]
    if first.startswith("I") and len(first) > 1 and first[1].isupper():
        return "", ", ".join(parts)
    return first, ", ".join(parts[1:])


def split_comma_safe(value: str) -> list[str]:
    result = []
    current = []
    depth = 0
    for ch in value:
        if ch == "<":
            depth += 1
        elif ch == ">":
            depth = max(0, depth - 1)
        if ch == "," and depth == 0:
            result.append("".join(current))
            current = []
        else:
            current.append(ch)
    if current:
        result.append("".join(current))
    return result


def accessibility_from_modifiers(modifiers: str) -> str:
    tokens = modifiers.split()
    values = [token for token in tokens if token in ACCESS]
    return " ".join(values)


def reference_tokens(lines: list[str], start_line: int, end_line: int):
    seen: set[tuple[str, int]] = set()
    for line_no in range(start_line, min(end_line, start_line + 120) + 1):
        if line_no - 1 >= len(lines):
            break
        line = strip_strings_and_comments(lines[line_no - 1])
        for token in TOKEN_RE.findall(line):
            if len(token) < 3 or token in MEMBER_KEYWORDS:
                continue
            if token[0].islower() and token not in {"api", "capi", "sapi"}:
                continue
            key = (token, line_no)
            if key in seen:
                continue
            seen.add(key)
            yield token, line_no
            if len(seen) >= 40:
                return


def extract_registration_calls(text: str) -> list[RegistrationDecl]:
    """Extracts only supported registration calls whose key and concrete type are explicit in source."""

    clean_text = strip_comments_preserve_layout(text)
    registrations = []
    for match in REGISTRATION_CALL_RE.finditer(clean_text):
        method_name = match.group("method")
        registrations.append(
            RegistrationDecl(
                method_name=method_name,
                registration_kind=REGISTRATION_METHOD_KINDS[method_name],
                registration_key=decode_csharp_string(match.group("key")),
                type_name=normalize_registered_type(match.group("type")),
                line=clean_text.count("\n", 0, match.start()) + 1,
                evidence=normalize_space(match.group(0)),
            )
        )
    return registrations


def strip_comments_preserve_layout(text: str) -> str:
    """Masks comments while preserving strings, character literals, offsets, and line numbers for evidence."""

    def replace_match(match: re.Match) -> str:
        if match.group("string") is not None:
            return match.group(0)

        # Newlines remain intact so extracted locations still address the original decompiled source file.
        return mask_preserve_layout(match.group(0))

    return COMMENT_OR_LITERAL_RE.sub(replace_match, text)


def decode_csharp_string(value: str) -> str:
    """Decodes the small escape subset relevant to registration identifiers without evaluating arbitrary text."""

    replacements = {
        r"\"": '"',
        r"\\": "\\",
        r"\n": "\n",
        r"\r": "\r",
        r"\t": "\t",
    }
    return re.sub(r'\\["\\nrt]', lambda match: replacements.get(match.group(0), match.group(0)), value)


def normalize_registered_type(value: str) -> str:
    """Normalizes harmless decompiler formatting while retaining the source-level type identity."""

    return normalize_space(value).removeprefix("global::")


def find_containing_member_id(line: int, member_locations: list[tuple[int, int, int]]) -> int | None:
    """Returns the narrowest indexed member spanning a registration line, avoiding guesses when none spans it."""

    return find_narrowest_span_id(line, member_locations)


def find_narrowest_span_id(line: int, locations: list[tuple[int, int, int]]) -> int | None:
    """Selects the most specific indexed declaration range containing a source line."""

    candidates = [location for location in locations if location[0] <= line <= location[1]]
    if not candidates:
        return None
    return min(candidates, key=lambda location: location[1] - location[0])[2]


def resolve_registration_types(conn: sqlite3.Connection) -> int:
    """Links registration ``typeof`` values to declarations only when scoring produces one unambiguous winner."""

    registrations = conn.execute(
        """
        SELECT reg.id, reg.root_id, reg.type_name, f.namespace_hint
        FROM registrations reg
        JOIN source_files f ON f.id = reg.file_id
        """
    ).fetchall()
    resolved = 0

    for registration_id, root_id, type_name, namespace_hint in registrations:
        simple_name = type_name.rsplit(".", 1)[-1]
        candidates = conn.execute(
            """
            SELECT id, root_id, namespace, name, full_name
            FROM types
            WHERE lower(name) = lower(?) OR lower(full_name) = lower(?)
            """,
            (simple_name, type_name),
        ).fetchall()
        scored = []
        for candidate in candidates:
            candidate_id, candidate_root_id, namespace, name, full_name = candidate
            score = 0
            if full_name.casefold() == type_name.casefold():
                score += 100
            if candidate_root_id == root_id:
                score += 30
            if namespace_hint and (namespace or "").casefold() == namespace_hint.casefold():
                score += 20
            if name.casefold() == simple_name.casefold():
                score += 10
            scored.append((score, candidate_id))

        scored.sort(reverse=True)
        if not scored or (len(scored) > 1 and scored[0][0] == scored[1][0]):
            continue

        conn.execute(
            "UPDATE registrations SET registered_type_id = ? WHERE id = ?",
            (scored[0][1], registration_id),
        )
        resolved += 1

    return resolved


def file_signature(path: Path, stat: os.stat_result, strong_hash: bool) -> str:
    if strong_hash:
        h = hashlib.sha256()
        with path.open("rb") as stream:
            for chunk in iter(lambda: stream.read(1024 * 1024), b""):
                h.update(chunk)
        return h.hexdigest()
    return f"{stat.st_size}:{stat.st_mtime_ns}"


def normalize_space(value: str) -> str:
    return re.sub(r"\s+", " ", value or "").strip()


def clean_symbol_name(value: str) -> str:
    return (value or "").strip().strip("`")


def compact_text(parts: list[str], limit: int = 12000) -> str:
    output = []
    total = 0
    for part in parts:
        text = normalize_space(str(part or ""))
        if not text:
            continue
        if total + len(text) + 1 > limit:
            remaining = limit - total
            if remaining > 0:
                output.append(text[:remaining])
            break
        output.append(text)
        total += len(text) + 1
    return " ".join(output)


def validate_db(db_path: Path) -> None:
    conn = sqlite3.connect(db_path)
    try:
        conn.execute("SELECT COUNT(*) FROM source_files").fetchone()
        conn.execute("SELECT COUNT(*) FROM types").fetchone()
        conn.execute("SELECT COUNT(*) FROM members").fetchone()
        conn.execute("SELECT COUNT(*) FROM registrations").fetchone()
        conn.execute("SELECT value FROM index_meta WHERE key = 'schema_version'").fetchone()
    finally:
        conn.close()


if __name__ == "__main__":
    raise SystemExit(main())
