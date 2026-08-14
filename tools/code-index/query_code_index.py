#!/usr/bin/env python3
import argparse
import sqlite3
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser(description="Query the Vintage Story Reference code index.")
    parser.add_argument("--db", required=True, help="Path to code.sqlite")
    parser.add_argument("--type", help="Type name or full name substring")
    parser.add_argument("--member", help="Member name substring")
    parser.add_argument("--search", help="FTS search text")
    parser.add_argument("--base", help="Base type substring")
    parser.add_argument("--root", help="Source root name substring")
    parser.add_argument("--limit", type=int, default=25)
    args = parser.parse_args()

    db_path = Path(args.db)
    if not db_path.is_file():
        raise SystemExit(f"Index not found: {db_path}")

    with sqlite3.connect(db_path) as conn:
        conn.row_factory = sqlite3.Row
        if args.member:
            rows = query_members(conn, args)
        elif args.search:
            rows = query_search(conn, args)
        else:
            rows = query_types(conn, args)

    for row in rows:
        print(format_row(row))
    return 0


def query_types(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = []
    params = []
    if args.type:
        clauses.append("(t.name LIKE ? OR t.full_name LIKE ?)")
        params.extend([f"%{args.type}%", f"%{args.type}%"])
    if args.base:
        clauses.append("(t.base_type LIKE ? OR t.interfaces LIKE ?)")
        params.extend([f"%{args.base}%", f"%{args.base}%"])
    if args.root:
        clauses.append("r.name LIKE ?")
        params.append(f"%{args.root}%")
    where = "WHERE " + " AND ".join(clauses) if clauses else ""
    sql = f"""
        SELECT r.name AS root, t.kind, t.full_name, t.base_type, t.interfaces, f.rel_path, t.start_line, t.end_line
        FROM types t
        JOIN source_files f ON f.id = t.file_id
        JOIN source_roots r ON r.id = t.root_id
        {where}
        ORDER BY t.full_name
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def query_members(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = ["m.name LIKE ?"]
    params = [f"%{args.member}%"]
    if args.type:
        clauses.append("(t.name LIKE ? OR t.full_name LIKE ?)")
        params.extend([f"%{args.type}%", f"%{args.type}%"])
    if args.root:
        clauses.append("r.name LIKE ?")
        params.append(f"%{args.root}%")
    where = "WHERE " + " AND ".join(clauses)
    sql = f"""
        SELECT r.name AS root, t.full_name AS type, m.kind, m.name, m.signature, f.rel_path, m.start_line, m.end_line
        FROM members m
        JOIN types t ON t.id = m.type_id
        JOIN source_files f ON f.id = m.file_id
        JOIN source_roots r ON r.id = m.root_id
        {where}
        ORDER BY t.full_name, m.start_line
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def query_search(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = ["code_text MATCH ?"]
    params = [args.search]
    if args.root:
        clauses.append("c.root_name LIKE ?")
        params.append(f"%{args.root}%")
    where = "WHERE " + " AND ".join(clauses)
    sql = f"""
        SELECT c.root_name AS root, c.rel_path, c.namespace, snippet(code_text, 3, '[', ']', ' ... ', 12) AS snippet
        FROM code_text c
        {where}
        ORDER BY c.rel_path
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def format_row(row: sqlite3.Row) -> str:
    return " | ".join(f"{key}={row[key]}" for key in row.keys())


if __name__ == "__main__":
    raise SystemExit(main())
