#!/usr/bin/env python3
"""
Builds a small SQLite full-text index for external web documentation that
belongs with a Vintage Story Reference version.

The existing asset and code indexes cover local game files and decompiled C#
sources. This companion index is for curated external documentation, such as a
GitHub wiki page that explains a mod's public JSON contract or a Mod DB page
that explains how a reference mod is meant to work. The builder fetches the
configured source pages, normalizes Markdown or selected HTML content into
heading-based sections, and stores the searchable knowledge in SQLite without
writing separate local page snapshots.
"""

import argparse
import datetime as dt
import hashlib
import json
import os
import re
import sqlite3
import urllib.request
from html.parser import HTMLParser
from pathlib import Path


SCHEMA_VERSION = 1
TOOL_VERSION = "1.1.0"
HEADING_RE = re.compile(r"^(?P<marks>#{1,6})\s+(?P<title>.+?)\s*#*\s*$")
MAIN_RE = re.compile(r"(?is)<main\b[^>]*>(?P<body>.*?)</main>")
BODY_RE = re.compile(r"(?is)<body\b[^>]*>(?P<body>.*?)</body>")
MODDB_DESCRIPTION_RE = re.compile(
    r"(?is)<div\b[^>]*class=[\"'][^\"']*\btab-content\b[^\"']*\bdescription\b[^\"']*[\"'][^>]*>"
    r"(?P<body>.*?)"
    r"<div\b[^>]*class=[\"'][^\"']*\btab-content\b[^\"']*\bfiles\b[^\"']*[\"'][^>]*>"
)


def main() -> int:
    parser = argparse.ArgumentParser(description="Build the Vintage Story Reference web documentation index.")
    parser.add_argument("--version", required=True, help="Vintage Story version folder, e.g. 1.22.3")
    parser.add_argument("--repo-root", default=".", help="Repository root")
    parser.add_argument("--sources", help="Path to web sources JSON; defaults to versions/<version>/Web/sources.json")
    args = parser.parse_args()

    repo_root = Path(args.repo_root).resolve()
    version_root = repo_root / "versions" / args.version
    index_dir = version_root / "Index"
    db_path = index_dir / "web.sqlite"
    tmp_db_path = index_dir / "web.sqlite.tmp"
    manifest_path = index_dir / "web.index.json"
    sources_path = Path(args.sources).resolve() if args.sources else version_root / "Web" / "sources.json"

    if not version_root.is_dir():
        raise SystemExit(f"Version root not found: {version_root}")
    if not sources_path.is_file():
        raise SystemExit(f"Web sources file not found: {sources_path}")

    sources = load_sources(sources_path)
    if not sources:
        raise SystemExit(f"No web sources configured in {sources_path}")

    index_dir.mkdir(parents=True, exist_ok=True)
    if tmp_db_path.exists():
        try:
            tmp_db_path.unlink()
        except PermissionError:
            tmp_db_path = index_dir / f"web.sqlite.{os.getpid()}.tmp"
            if tmp_db_path.exists():
                tmp_db_path.unlink()

    stats = {
        "sources": 0,
        "sections": 0,
        "bytes": 0,
        "characters": 0,
    }
    indexed_sources: list[dict[str, object]] = []

    conn = sqlite3.connect(tmp_db_path)
    try:
        create_schema(conn)

        for source in sources:
            indexed_source = index_source(
                conn=conn,
                source=source,
                game_version=args.version,
                stats=stats,
            )
            indexed_sources.append(indexed_source)

        conn.commit()
        write_meta(conn, args.version, sources_path, stats)
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
        "game_version": args.version,
        "sources_path": str(sources_path),
        "index_path": str(db_path),
        "stats": stats,
        "sources": indexed_sources,
        "limitations": [
            "This index contains curated external documentation, not local game assets or decompiled code.",
            "Source pages are fetched at build time and represented as dated index content.",
            "Use the source URL for current upstream truth when recency matters.",
        ],
    }
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")

    print(f"Indexed {stats['sources']} web source(s) and {stats['sections']} section(s) into {db_path}")
    print(f"Manifest: {manifest_path}")
    return 0



def load_sources(path: Path) -> list[dict[str, object]]:
    data = json.loads(path.read_text(encoding="utf-8"))
    sources = data.get("sources", data if isinstance(data, list) else [])
    if not isinstance(sources, list):
        raise SystemExit(f"Expected a source list in {path}")
    return sources



def create_schema(conn: sqlite3.Connection) -> None:
    conn.executescript(
        """
        CREATE TABLE index_meta (
            key TEXT PRIMARY KEY,
            value TEXT NOT NULL
        );

        CREATE TABLE sources (
            id INTEGER PRIMARY KEY,
            game_version TEXT NOT NULL,
            slug TEXT NOT NULL UNIQUE,
            title TEXT NOT NULL,
            site TEXT,
            source_type TEXT NOT NULL,
            url TEXT NOT NULL,
            raw_url TEXT NOT NULL,
            retrieved_at_utc TEXT NOT NULL,
            last_known_edited TEXT,
            applies_to TEXT,
            notes TEXT,
            content_hash TEXT NOT NULL,
            byte_count INTEGER NOT NULL,
            character_count INTEGER NOT NULL
        );

        CREATE TABLE sections (
            id INTEGER PRIMARY KEY,
            source_id INTEGER NOT NULL,
            slug TEXT NOT NULL,
            title TEXT NOT NULL,
            heading_level INTEGER NOT NULL,
            heading_path TEXT NOT NULL,
            start_line INTEGER NOT NULL,
            end_line INTEGER NOT NULL,
            body TEXT NOT NULL,
            FOREIGN KEY(source_id) REFERENCES sources(id)
        );

        CREATE INDEX idx_sections_source ON sections(source_id);
        CREATE INDEX idx_sections_heading ON sections(heading_path);

        CREATE VIRTUAL TABLE web_text USING fts5(
            source_slug,
            source_title,
            heading_path,
            body
        );
        """
    )



def index_source(
    conn: sqlite3.Connection,
    source: dict[str, object],
    game_version: str,
    stats: dict[str, int],
) -> dict[str, object]:
    slug = required_text(source, "slug")
    title = required_text(source, "title")
    url = required_text(source, "url")
    raw_url = required_text(source, "raw_url")
    site = optional_text(source, "site")
    source_type = optional_text(source, "source_type") or "markdown"
    last_known_edited = optional_text(source, "last_known_edited")
    applies_to = optional_text(source, "applies_to")
    notes = optional_text(source, "notes")

    content = fetch_text(raw_url, source_type)
    content_hash = hashlib.sha256(content.encode("utf-8")).hexdigest()
    retrieved_at_utc = dt.datetime.now(dt.timezone.utc).isoformat()
    normalized_content = normalize_source_content(
        content=content,
        source_type=source_type,
        title=title,
        html_scope=optional_text(source, "html_scope"),
    )
    sections = parse_markdown_sections(normalized_content)

    source_cursor = conn.execute(
        """
        INSERT INTO sources(
            game_version, slug, title, site, source_type, url, raw_url, retrieved_at_utc,
            last_known_edited, applies_to, notes, content_hash, byte_count, character_count
        )
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """,
        (
            game_version,
            slug,
            title,
            site,
            source_type,
            url,
            raw_url,
            retrieved_at_utc,
            last_known_edited,
            applies_to,
            notes,
            content_hash,
            len(content.encode("utf-8")),
            len(content),
        ),
    )
    source_id = source_cursor.lastrowid

    for section in sections:
        section_cursor = conn.execute(
            """
            INSERT INTO sections(
                source_id, slug, title, heading_level, heading_path, start_line, end_line, body
            )
            VALUES (?, ?, ?, ?, ?, ?, ?, ?)
            """,
            (
                source_id,
                section["slug"],
                section["title"],
                section["heading_level"],
                section["heading_path"],
                section["start_line"],
                section["end_line"],
                section["body"],
            ),
        )
        section_id = section_cursor.lastrowid
        conn.execute(
            "INSERT INTO web_text(rowid, source_slug, source_title, heading_path, body) VALUES (?, ?, ?, ?, ?)",
            (
                section_id,
                slug,
                title,
                section["heading_path"],
                section["body"],
            ),
        )

    stats["sources"] += 1
    stats["sections"] += len(sections)
    stats["bytes"] += len(content.encode("utf-8"))
    stats["characters"] += len(content)

    return {
        "slug": slug,
        "title": title,
        "url": url,
        "raw_url": raw_url,
        "retrieved_at_utc": retrieved_at_utc,
        "last_known_edited": last_known_edited,
        "applies_to": applies_to,
        "content_hash": content_hash,
        "byte_count": len(content.encode("utf-8")),
        "character_count": len(content),
        "section_count": len(sections),
    }



def fetch_text(url: str, source_type: str) -> str:
    accept_header = "text/html, application/xhtml+xml, */*" if "html" in source_type else "text/plain, text/markdown, */*"
    request = urllib.request.Request(
        url,
        headers={
            "Accept": accept_header,
            "User-Agent": f"VintageStoryReferenceWebIndexer/{TOOL_VERSION}",
        },
    )
    with urllib.request.urlopen(request, timeout=30) as response:
        content = response.read()
    return content.decode("utf-8-sig", errors="replace")



def normalize_source_content(content: str, source_type: str, title: str, html_scope: str | None) -> str:
    if "html" in source_type:
        scoped_html = extract_html_scope(content, html_scope)
        return html_to_markdown_text(scoped_html, title)
    return content



def extract_html_scope(content: str, html_scope: str | None) -> str:
    if html_scope == "moddb_description":
        match = MODDB_DESCRIPTION_RE.search(content)
        if match:
            return match.group("body")

    main_match = MAIN_RE.search(content)
    if main_match:
        return main_match.group("body")

    body_match = BODY_RE.search(content)
    if body_match:
        return body_match.group("body")

    return content



def html_to_markdown_text(content: str, title: str) -> str:
    parser = TextHtmlParser()
    parser.feed(content)
    parser.close()
    text = parser.to_text()

    lines = [f"# {title}"]
    for line in text.splitlines():
        cleaned = re.sub(r"\s+", " ", line).strip()
        if not cleaned or cleaned == "?>":
            continue
        lines.append(cleaned)
    return "\n".join(lines)



class TextHtmlParser(HTMLParser):
    def __init__(self) -> None:
        super().__init__(convert_charrefs=True)
        self._chunks: list[str] = []
        self._heading_level: int | None = None
        self._skip_depth = 0



    def handle_starttag(self, tag: str, attrs: list[tuple[str, str | None]]) -> None:
        tag = tag.lower()
        if tag in {"script", "style", "svg", "canvas", "button"}:
            self._skip_depth += 1
            return
        if self._skip_depth:
            return

        if tag in {"h1", "h2", "h3", "h4", "h5", "h6"}:
            self._line_break()
            self._heading_level = int(tag[1])
            self._chunks.append("#" * self._heading_level + " ")
            return
        if tag == "li":
            self._line_break()
            self._chunks.append("- ")
            return
        if tag in {"p", "div", "section", "article", "tr", "dt", "dd"}:
            self._line_break()
            return
        if tag == "br":
            self._line_break()



    def handle_endtag(self, tag: str) -> None:
        tag = tag.lower()
        if tag in {"script", "style", "svg", "canvas", "button"}:
            self._skip_depth = max(0, self._skip_depth - 1)
            return
        if self._skip_depth:
            return

        if tag in {"h1", "h2", "h3", "h4", "h5", "h6"}:
            self._heading_level = None
            self._line_break()
            return
        if tag in {"p", "div", "section", "article", "li", "tr", "dt", "dd"}:
            self._line_break()



    def handle_data(self, data: str) -> None:
        if self._skip_depth:
            return
        cleaned = re.sub(r"\s+", " ", data)
        if cleaned.strip():
            self._chunks.append(cleaned)
            self._chunks.append(" ")



    def to_text(self) -> str:
        text = "".join(self._chunks)
        text = re.sub(r"[ \t]+\n", "\n", text)
        text = re.sub(r"\n{3,}", "\n\n", text)
        return text.strip()



    def _line_break(self) -> None:
        if not self._chunks:
            return
        current = "".join(self._chunks)
        if not current.endswith("\n"):
            self._chunks.append("\n")



def parse_markdown_sections(markdown: str) -> list[dict[str, object]]:
    lines = markdown.splitlines()
    sections: list[dict[str, object]] = []
    heading_stack: list[str] = []
    current: dict[str, object] | None = None
    body_lines: list[str] = []
    in_fence = False

    for index, line in enumerate(lines, start=1):
        fence_marker = line.lstrip()
        if fence_marker.startswith("```") or fence_marker.startswith("~~~"):
            in_fence = not in_fence

        match = None if in_fence else HEADING_RE.match(line)
        if match:
            if current is not None:
                current["end_line"] = index - 1
                current["body"] = "\n".join(body_lines).strip()
                sections.append(current)

            level = len(match.group("marks"))
            title = clean_heading(match.group("title"))
            heading_stack = heading_stack[: level - 1]
            heading_stack.append(title)
            current = {
                "slug": slugify(" ".join(heading_stack)),
                "title": title,
                "heading_level": level,
                "heading_path": " > ".join(heading_stack),
                "start_line": index,
                "end_line": index,
                "body": "",
            }
            body_lines = []
            continue

        if current is None and line.strip():
            current = {
                "slug": "document",
                "title": "Document",
                "heading_level": 0,
                "heading_path": "Document",
                "start_line": index,
                "end_line": index,
                "body": "",
            }
        if current is not None:
            body_lines.append(line)

    if current is not None:
        current["end_line"] = len(lines)
        current["body"] = "\n".join(body_lines).strip()
        sections.append(current)

    return [section for section in sections if str(section["body"]).strip() or section["heading_level"] == 1]



def clean_heading(value: str) -> str:
    return re.sub(r"\s+", " ", value.strip().strip("#").strip())



def slugify(value: str) -> str:
    slug = re.sub(r"[^a-z0-9]+", "-", value.lower()).strip("-")
    return slug or "section"



def required_text(source: dict[str, object], key: str) -> str:
    value = optional_text(source, key)
    if not value:
        raise SystemExit(f"Configured web source is missing required field: {key}")
    return value



def optional_text(source: dict[str, object], key: str) -> str | None:
    value = source.get(key)
    if value is None:
        return None
    text = str(value).strip()
    return text or None



def write_meta(conn: sqlite3.Connection, version: str, sources_path: Path, stats: dict[str, int]) -> None:
    values = {
        "schema_version": str(SCHEMA_VERSION),
        "tool_version": TOOL_VERSION,
        "game_version": version,
        "sources_path": str(sources_path),
        "generated_at_utc": dt.datetime.now(dt.timezone.utc).isoformat(),
        "sources": str(stats["sources"]),
        "sections": str(stats["sections"]),
        "bytes": str(stats["bytes"]),
        "characters": str(stats["characters"]),
    }
    conn.executemany("INSERT INTO index_meta(key, value) VALUES (?, ?)", values.items())



def validate_db(db_path: Path) -> None:
    conn = sqlite3.connect(db_path)
    try:
        conn.execute("SELECT COUNT(*) FROM sources").fetchone()
        conn.execute("SELECT COUNT(*) FROM sections").fetchone()
        conn.execute("SELECT value FROM index_meta WHERE key = 'schema_version'").fetchone()
    finally:
        conn.close()


if __name__ == "__main__":
    raise SystemExit(main())
