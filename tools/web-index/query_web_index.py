#!/usr/bin/env python3
"""
Queries the curated external web documentation index for Vintage Story
Reference.

The command mirrors the compact style of the existing asset and code query
helpers. It is intended for agents that need to locate relevant documentation
sections before opening upstream pages or implementation files.
"""

import argparse
import re
import sqlite3
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser(description="Query the Vintage Story Reference web documentation index.")
    parser.add_argument("--db", required=True, help="Path to web.sqlite")
    parser.add_argument("--search", help="Search text; punctuation is normalized for SQLite FTS")
    parser.add_argument("--source", help="Filter by source slug or title substring")
    parser.add_argument("--heading", help="Filter by heading path substring")
    parser.add_argument("--list-sources", action="store_true", help="List indexed web documentation sources")
    parser.add_argument("--limit", type=int, default=25)
    args = parser.parse_args()

    db_path = Path(args.db)
    if not db_path.is_file():
        raise SystemExit(f"Index not found: {db_path}")

    with sqlite3.connect(db_path) as conn:
        conn.row_factory = sqlite3.Row
        if args.list_sources:
            rows = list_sources(conn, args.limit)
        else:
            rows = query_sections(conn, args)

    for row in rows:
        print(format_row(row))
    return 0



def list_sources(conn: sqlite3.Connection, limit: int) -> list[sqlite3.Row]:
    sql = """
        SELECT
            s.slug,
            s.title,
            s.url,
            s.retrieved_at_utc,
            s.last_known_edited,
            s.applies_to,
            s.content_hash,
            COUNT(sec.id) AS sections
        FROM sources s
        LEFT JOIN sections sec ON sec.source_id = s.id
        GROUP BY s.id
        ORDER BY s.slug
        LIMIT ?
    """
    return conn.execute(sql, (limit,)).fetchall()



def query_sections(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = []
    params = []
    join = ""
    select_snippet = "substr(sec.body, 1, 240) AS snippet"

    if args.search:
        join = "JOIN web_text wt ON wt.rowid = sec.id"
        clauses.append("web_text MATCH ?")
        params.append(fts_query(args.search))
        select_snippet = "snippet(web_text, 3, '[', ']', ' ... ', 18) AS snippet"

    if args.source:
        clauses.append("(s.slug LIKE ? OR s.title LIKE ?)")
        params.extend([f"%{args.source}%", f"%{args.source}%"])

    if args.heading:
        clauses.append("sec.heading_path LIKE ?")
        params.append(f"%{args.heading}%")

    where = "WHERE " + " AND ".join(clauses) if clauses else ""
    sql = f"""
        SELECT
            s.slug AS source,
            s.title AS source_title,
            sec.heading_path,
            sec.start_line,
            sec.end_line,
            s.url,
            {select_snippet}
        FROM sections sec
        JOIN sources s ON s.id = sec.source_id
        {join}
        {where}
        ORDER BY s.slug, sec.start_line
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()



def fts_query(value: str) -> str:
    tokens = re.findall(r"[A-Za-z0-9_]+", value)
    if not tokens:
        return quote_fts(value)
    return " AND ".join(quote_fts(token) for token in tokens)



def quote_fts(value: str) -> str:
    return '"' + value.replace('"', '""') + '"'



def format_row(row: sqlite3.Row) -> str:
    return " | ".join(f"{key}={row[key]}" for key in row.keys())


if __name__ == "__main__":
    raise SystemExit(main())
