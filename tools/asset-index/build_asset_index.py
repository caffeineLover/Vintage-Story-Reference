#!/usr/bin/env python3
import argparse
import datetime as dt
import hashlib
import json
import os
import re
import shutil
import sqlite3
import struct
from pathlib import Path


SCHEMA_VERSION = 1
TOOL_VERSION = "1.0.2"
COMMON_CATEGORIES = {
    "blocktypes",
    "itemtypes",
    "entities",
    "recipes",
    "patches",
    "lang",
    "shapes",
    "textures",
    "sounds",
    "config",
    "worldproperties",
    "shader",
    "shaders",
}
BINARY_EXTENSIONS = {
    ".png",
    ".ogg",
    ".ttf",
    ".svg",
    ".fsh",
    ".vsh",
    ".ash",
    ".obj",
    ".gltf",
}
FACT_KEYS = {
    "code",
    "class",
    "entityClass",
    "shape",
    "textures",
    "variantgroups",
    "variantGroups",
    "variants",
    "output",
    "ingredients",
    "ingredientPattern",
    "path",
    "op",
    "value",
    "file",
    "name",
    "type",
    "side",
    "dependsOn",
    "attributes",
}


def main() -> int:
    parser = argparse.ArgumentParser(description="Build the Vintage Story Reference asset SQLite index.")
    parser.add_argument("--version", required=True, help="Vintage Story version folder, e.g. 1.22.3")
    parser.add_argument("--repo-root", default=".", help="Repository root")
    parser.add_argument("--strong-hash", action="store_true", help="Hash all file contents instead of cheap metadata signatures")
    args = parser.parse_args()

    repo_root = Path(args.repo_root).resolve()
    version = args.version
    version_root = repo_root / "versions" / version
    asset_root = version_root / "Assets"
    asset_roots = discover_asset_roots(version_root)
    index_dir = repo_root / "versions" / version / "Index"
    db_path = index_dir / "assets.sqlite"
    tmp_db_path = index_dir / "assets.sqlite.tmp"
    manifest_path = index_dir / "assets.index.json"

    if not asset_roots:
        raise SystemExit(f"No asset roots found for version {version}")

    index_dir.mkdir(parents=True, exist_ok=True)
    if tmp_db_path.exists():
        try:
            tmp_db_path.unlink()
        except PermissionError:
            tmp_db_path = index_dir / f"assets.sqlite.{os.getpid()}.tmp"
            if tmp_db_path.exists():
                tmp_db_path.unlink()

    stats = {
        "total_files": 0,
        "json_files": 0,
        "json_parse_ok": 0,
        "json_parse_error": 0,
        "binary_files": 0,
        "total_bytes": 0,
        "json_bytes": 0,
        "binary_bytes": 0,
    }
    extension_counts: dict[str, int] = {}
    category_counts: dict[str, int] = {}
    domain_counts: dict[str, int] = {}
    root_counts: dict[str, int] = {}
    parse_errors: list[dict[str, str]] = []

    conn = sqlite3.connect(tmp_db_path)
    try:
        conn.execute("PRAGMA journal_mode = OFF")
        conn.execute("PRAGMA synchronous = OFF")
        conn.execute("PRAGMA temp_store = MEMORY")
        create_schema(conn)

        for current_asset_root in asset_roots:
            root_key = current_asset_root.relative_to(repo_root).as_posix()
            for path in iter_files(current_asset_root):
                index_file(
                    conn=conn,
                    repo_root=repo_root,
                    asset_root=current_asset_root,
                    path=path,
                    version=version,
                    stats=stats,
                    extension_counts=extension_counts,
                    category_counts=category_counts,
                    domain_counts=domain_counts,
                    parse_errors=parse_errors,
                    strong_hash=args.strong_hash,
                )
                root_counts[root_key] = root_counts.get(root_key, 0) + 1

        conn.commit()
        write_meta(conn, version, asset_roots, stats)
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
        "generated_at_utc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "repo_root": str(repo_root),
        "game_version": version,
        "asset_root": str(asset_root),
        "asset_roots": [str(root) for root in asset_roots],
        "index_path": str(db_path),
        "strong_hash": bool(args.strong_hash),
        "stats": stats,
        "root_counts": dict(sorted(root_counts.items())),
        "extension_counts": dict(sorted(extension_counts.items())),
        "domain_counts": dict(sorted(domain_counts.items())),
        "category_counts": dict(sorted(category_counts.items())),
        "parse_errors_sample": parse_errors[:50],
    }
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")

    print(f"Indexed {stats['total_files']} files into {db_path}")
    print(f"JSON parsed: {stats['json_parse_ok']} ok, {stats['json_parse_error']} errors")
    print(f"Manifest: {manifest_path}")
    return 0


def discover_asset_roots(version_root: Path) -> list[Path]:
    roots: list[Path] = []
    vanilla_root = version_root / "Assets"
    if vanilla_root.is_dir():
        roots.append(vanilla_root)

    mods_root = version_root / "mods"
    if mods_root.is_dir():
        for mod_dir in sorted(path for path in mods_root.iterdir() if path.is_dir()):
            assets_dir = mod_dir / "assets"
            if assets_dir.is_dir():
                roots.append(assets_dir)

    return roots


def create_schema(conn: sqlite3.Connection) -> None:
    conn.executescript(
        """
        CREATE TABLE index_meta (
            key TEXT PRIMARY KEY,
            value TEXT NOT NULL
        );

        CREATE TABLE assets (
            id INTEGER PRIMARY KEY,
            version TEXT NOT NULL,
            rel_path TEXT NOT NULL UNIQUE,
            domain TEXT,
            category TEXT,
            subcategory TEXT,
            extension TEXT NOT NULL,
            size INTEGER NOT NULL,
            mtime_ns INTEGER NOT NULL,
            signature TEXT NOT NULL,
            is_json INTEGER NOT NULL,
            parse_status TEXT NOT NULL,
            png_width INTEGER,
            png_height INTEGER
        );

        CREATE INDEX idx_assets_domain_category ON assets(domain, category);
        CREATE INDEX idx_assets_extension ON assets(extension);
        CREATE INDEX idx_assets_rel_path ON assets(rel_path);

        CREATE TABLE json_assets (
            id INTEGER PRIMARY KEY,
            asset_id INTEGER NOT NULL,
            fact_key TEXT NOT NULL,
            fact_value TEXT NOT NULL,
            value_type TEXT NOT NULL,
            json_path TEXT NOT NULL,
            FOREIGN KEY(asset_id) REFERENCES assets(id)
        );

        CREATE INDEX idx_json_assets_key_value ON json_assets(fact_key, fact_value);
        CREATE INDEX idx_json_assets_asset ON json_assets(asset_id);

        CREATE VIRTUAL TABLE asset_text USING fts5(
            rel_path,
            domain,
            category,
            text
        );
        """
    )


def write_meta(conn: sqlite3.Connection, version: str, asset_roots: list[Path], stats: dict[str, int]) -> None:
    values = {
        "schema_version": str(SCHEMA_VERSION),
        "tool_version": TOOL_VERSION,
        "game_version": version,
        "asset_root": str(asset_roots[0]) if asset_roots else "",
        "asset_roots": json.dumps([str(root) for root in asset_roots]),
        "generated_at_utc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "total_files": str(stats["total_files"]),
        "json_files": str(stats["json_files"]),
        "json_parse_ok": str(stats["json_parse_ok"]),
        "json_parse_error": str(stats["json_parse_error"]),
    }
    conn.executemany("INSERT INTO index_meta(key, value) VALUES (?, ?)", values.items())


def iter_files(root: Path):
    for dirpath, dirnames, filenames in os.walk(root):
        dirnames.sort()
        for filename in sorted(filenames):
            yield Path(dirpath) / filename


def index_file(
    conn: sqlite3.Connection,
    repo_root: Path,
    asset_root: Path,
    path: Path,
    version: str,
    stats: dict[str, int],
    extension_counts: dict[str, int],
    category_counts: dict[str, int],
    domain_counts: dict[str, int],
    parse_errors: list[dict[str, str]],
    strong_hash: bool,
) -> None:
    rel_asset_path = path.relative_to(asset_root).as_posix()
    rel_repo_path = path.relative_to(repo_root).as_posix()
    parts = rel_asset_path.split("/")
    domain = parts[0] if parts else None
    category = detect_category(parts)
    subcategory = detect_subcategory(parts, category)
    ext = path.suffix.lower()
    stat = path.stat()
    size = stat.st_size
    signature = file_signature(path, stat, strong_hash)
    png_width, png_height = png_dimensions(path) if ext == ".png" else (None, None)
    is_json = ext == ".json"
    parse_status = "not_json"

    stats["total_files"] += 1
    stats["total_bytes"] += size
    extension_counts[ext or "(none)"] = extension_counts.get(ext or "(none)", 0) + 1
    if domain:
        domain_counts[domain] = domain_counts.get(domain, 0) + 1
    if category:
        category_counts[category] = category_counts.get(category, 0) + 1
    if ext in BINARY_EXTENSIONS:
        stats["binary_files"] += 1
        stats["binary_bytes"] += size

    facts: list[tuple[str, str, str, str]] = []
    text_parts = [rel_repo_path, domain or "", category or "", subcategory or "", ext]

    if is_json:
        stats["json_files"] += 1
        stats["json_bytes"] += size
        try:
            value = load_json_lenient(path)
            parse_status = "ok"
            stats["json_parse_ok"] += 1
            facts = extract_facts(value)
            text_parts.extend(text_for_json(value, facts))
        except Exception as exc:
            parse_status = f"error: {type(exc).__name__}: {str(exc)[:160]}"
            stats["json_parse_error"] += 1
            if len(parse_errors) < 100:
                parse_errors.append({"path": rel_repo_path, "error": parse_status})
    else:
        if ext in {".svg", ".fsh", ".vsh", ".ash", ".txt", ".gltf", ".obj"} and size <= 512 * 1024:
            try:
                text_parts.append(path.read_text(encoding="utf-8", errors="replace")[:4000])
            except OSError:
                pass

    cur = conn.execute(
        """
        INSERT INTO assets(version, rel_path, domain, category, subcategory, extension, size, mtime_ns, signature,
                           is_json, parse_status, png_width, png_height)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """,
        (
            version,
            rel_repo_path,
            domain,
            category,
            subcategory,
            ext,
            size,
            stat.st_mtime_ns,
            signature,
            1 if is_json else 0,
            parse_status,
            png_width,
            png_height,
        ),
    )
    asset_id = cur.lastrowid

    if facts:
        conn.executemany(
            "INSERT INTO json_assets(asset_id, fact_key, fact_value, value_type, json_path) VALUES (?, ?, ?, ?, ?)",
            [(asset_id, key, value, value_type, json_path) for key, value, value_type, json_path in facts],
        )

    compact_text = compact_join(text_parts + [fact[1] for fact in facts], limit=12000)
    conn.execute(
        "INSERT INTO asset_text(rowid, rel_path, domain, category, text) VALUES (?, ?, ?, ?, ?)",
        (asset_id, rel_repo_path, domain or "", category or "", compact_text),
    )


def detect_category(parts: list[str]) -> str | None:
    for part in parts:
        if part in COMMON_CATEGORIES:
            return part
    return parts[1] if len(parts) > 1 else None


def detect_subcategory(parts: list[str], category: str | None) -> str | None:
    if not category:
        return None
    try:
        idx = parts.index(category)
    except ValueError:
        return None
    return parts[idx + 1] if len(parts) > idx + 2 else None


def file_signature(path: Path, stat: os.stat_result, strong_hash: bool) -> str:
    if strong_hash:
        h = hashlib.sha256()
        with path.open("rb") as stream:
            for chunk in iter(lambda: stream.read(1024 * 1024), b""):
                h.update(chunk)
        return h.hexdigest()
    return f"{stat.st_size}:{stat.st_mtime_ns}"


def png_dimensions(path: Path) -> tuple[int | None, int | None]:
    try:
        with path.open("rb") as stream:
            header = stream.read(24)
        if header[:8] != b"\x89PNG\r\n\x1a\n" or header[12:16] != b"IHDR":
            return None, None
        width, height = struct.unpack(">II", header[16:24])
        return int(width), int(height)
    except OSError:
        return None, None


def load_json_lenient(path: Path):
    text = path.read_text(encoding="utf-8-sig", errors="replace")
    text = strip_json_comments(text)
    text = quote_unquoted_object_keys(text)
    text = remove_trailing_commas(text)
    return json.loads(text)


def strip_json_comments(text: str) -> str:
    result = []
    i = 0
    in_string = False
    escape = False
    while i < len(text):
        ch = text[i]
        nxt = text[i + 1] if i + 1 < len(text) else ""
        if in_string:
            result.append(ch)
            if escape:
                escape = False
            elif ch == "\\":
                escape = True
            elif ch == '"':
                in_string = False
            i += 1
            continue
        if ch == '"':
            in_string = True
            result.append(ch)
            i += 1
            continue
        if ch == "/" and nxt == "/":
            i += 2
            while i < len(text) and text[i] not in "\r\n":
                i += 1
            continue
        if ch == "/" and nxt == "*":
            i += 2
            while i + 1 < len(text) and not (text[i] == "*" and text[i + 1] == "/"):
                i += 1
            i += 2
            continue
        result.append(ch)
        i += 1
    return "".join(result)


def remove_trailing_commas(text: str) -> str:
    result = []
    i = 0
    in_string = False
    escape = False
    while i < len(text):
        ch = text[i]
        if in_string:
            result.append(ch)
            if escape:
                escape = False
            elif ch == "\\":
                escape = True
            elif ch == '"':
                in_string = False
            i += 1
            continue
        if ch == '"':
            in_string = True
            result.append(ch)
            i += 1
            continue
        if ch == ",":
            j = i + 1
            while j < len(text) and text[j].isspace():
                j += 1
            if j < len(text) and text[j] in "]}":
                i += 1
                continue
        result.append(ch)
        i += 1
    return "".join(result)


def quote_unquoted_object_keys(text: str) -> str:
    result = []
    i = 0
    in_string = False
    escape = False
    expecting_key = False
    while i < len(text):
        ch = text[i]
        if in_string:
            result.append(ch)
            if escape:
                escape = False
            elif ch == "\\":
                escape = True
            elif ch == '"':
                in_string = False
            i += 1
            continue

        if ch == '"':
            in_string = True
            result.append(ch)
            i += 1
            continue

        if ch in "{,":
            expecting_key = True
            result.append(ch)
            i += 1
            continue

        if expecting_key and ch.isspace():
            result.append(ch)
            i += 1
            continue

        if expecting_key and (ch.isalpha() or ch == "_"):
            j = i + 1
            while j < len(text) and (text[j].isalnum() or text[j] in "_"):
                j += 1
            k = j
            while k < len(text) and text[k].isspace():
                k += 1
            if k < len(text) and text[k] == ":":
                result.append('"')
                result.append(text[i:j])
                result.append('"')
                i = j
                expecting_key = False
                continue

        if not ch.isspace():
            expecting_key = False
        result.append(ch)
        i += 1
    return "".join(result)


def extract_facts(value) -> list[tuple[str, str, str, str]]:
    facts: list[tuple[str, str, str, str]] = []

    def visit(node, path: str, key_hint: str | None = None) -> None:
        if len(facts) >= 500:
            return
        if isinstance(node, dict):
            for key, child in node.items():
                child_path = f"{path}.{key}" if path else key
                if key in FACT_KEYS:
                    add_fact(facts, key, child, child_path)
                elif key in {"base", "inheritFrom", "collectible", "transitionableProps"}:
                    add_fact(facts, key, child, child_path)
                visit(child, child_path, key)
        elif isinstance(node, list):
            for idx, child in enumerate(node[:200]):
                visit(child, f"{path}[{idx}]", key_hint)
        elif isinstance(node, str) and key_hint in {"shape", "texture", "textures", "path", "file", "output", "code"}:
            add_fact(facts, key_hint, node, path)

    visit(value, "$")
    return dedupe_facts(facts)


def add_fact(facts: list[tuple[str, str, str, str]], key: str, value, json_path: str) -> None:
    if isinstance(value, (str, int, float, bool)) or value is None:
        fact_value = scalar_to_text(value)
        if fact_value:
            facts.append((key, truncate(fact_value, 512), type(value).__name__, json_path))
        return
    if isinstance(value, list):
        for idx, item in enumerate(value[:100]):
            add_fact(facts, key, item, f"{json_path}[{idx}]")
        return
    if isinstance(value, dict):
        for child_key, child_value in list(value.items())[:100]:
            child_path = f"{json_path}.{child_key}"
            if isinstance(child_value, (dict, list)):
                add_fact(facts, key, child_value, child_path)
            else:
                combined = f"{child_key}:{scalar_to_text(child_value)}"
                facts.append((key, truncate(combined, 512), type(child_value).__name__, child_path))


def scalar_to_text(value) -> str:
    if value is None:
        return ""
    return str(value)


def dedupe_facts(facts: list[tuple[str, str, str, str]]) -> list[tuple[str, str, str, str]]:
    seen = set()
    result = []
    for fact in facts:
        key = (fact[0], fact[1], fact[3])
        if key in seen:
            continue
        seen.add(key)
        result.append(fact)
    return result


def text_for_json(value, facts: list[tuple[str, str, str, str]]) -> list[str]:
    text = [fact[1] for fact in facts[:300]]
    if isinstance(value, dict):
        text.extend(str(key) for key in list(value.keys())[:200])
        if "lang" in text:
            text.extend(flatten_strings(value, limit=200))
    return text


def flatten_strings(value, limit: int) -> list[str]:
    result = []

    def visit(node):
        if len(result) >= limit:
            return
        if isinstance(node, dict):
            for key, child in node.items():
                result.append(str(key))
                visit(child)
                if len(result) >= limit:
                    break
        elif isinstance(node, list):
            for child in node:
                visit(child)
                if len(result) >= limit:
                    break
        elif isinstance(node, str):
            result.append(node[:200])

    visit(value)
    return result


def compact_join(values, limit: int) -> str:
    pieces = []
    total = 0
    for value in values:
        if value is None:
            continue
        text = str(value).replace("\x00", " ").strip()
        if not text:
            continue
        if total + len(text) + 1 > limit:
            remaining = limit - total
            if remaining > 0:
                pieces.append(text[:remaining])
            break
        pieces.append(text)
        total += len(text) + 1
    return " ".join(pieces)


def truncate(value: str, limit: int) -> str:
    return value if len(value) <= limit else value[:limit]


def validate_db(db_path: Path) -> None:
    conn = sqlite3.connect(db_path)
    try:
        conn.execute("SELECT COUNT(*) FROM assets").fetchone()
        conn.execute("SELECT COUNT(*) FROM asset_text").fetchone()
        conn.execute("SELECT value FROM index_meta WHERE key = 'schema_version'").fetchone()
    finally:
        conn.close()


if __name__ == "__main__":
    raise SystemExit(main())
