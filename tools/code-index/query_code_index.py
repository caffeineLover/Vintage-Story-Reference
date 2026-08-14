#!/usr/bin/env python3
"""Queries symbol, reference-hint, and literal class-registration evidence from a generated code index."""

import argparse
import json
import sqlite3
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser(description="Query the Vintage Story Reference code index.")
    parser.add_argument("--db", required=True, help="Path to code.sqlite")
    parser.add_argument("--type", help="Type name or full name substring")
    parser.add_argument("--member", help="Member name substring")
    parser.add_argument("--reference", help="Referenced identifier substring from indexed member bodies")
    parser.add_argument(
        "--registration",
        help="Registration key, registered type, or registration method substring",
    )
    parser.add_argument("--registration-kind", help="Exact registration kind, such as block or block_entity")
    parser.add_argument("--registration-method", help="Exact registration method, such as RegisterBlockClass")
    parser.add_argument("--search", help="FTS search text")
    parser.add_argument("--base", help="Base type substring")
    parser.add_argument("--root", help="Source root name substring")
    parser.add_argument("--format", choices=("text", "json"), default="text")
    parser.add_argument("--limit", type=int, default=25)
    args = parser.parse_args()

    db_path = Path(args.db)
    if not db_path.is_file():
        raise SystemExit(f"Index not found: {db_path}")

    with sqlite3.connect(db_path) as conn:
        conn.row_factory = sqlite3.Row
        if args.registration or args.registration_kind or args.registration_method:
            rows = query_registrations(conn, args)
        elif args.member:
            rows = query_members(conn, args)
        elif args.reference:
            rows = query_references(conn, args)
        elif args.search:
            rows = query_search(conn, args)
        else:
            rows = query_types(conn, args)

    if args.format == "json":
        print(json.dumps([dict(row) for row in rows], indent=2))
    else:
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


def query_references(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    """Returns bounded identifier hints already extracted from indexed member bodies."""

    clauses = ["rh.token LIKE ?"]
    params = [f"%{args.reference}%"]
    if args.type:
        clauses.append("(t.name LIKE ? OR t.full_name LIKE ?)")
        params.extend([f"%{args.type}%", f"%{args.type}%"])
    if args.root:
        clauses.append("r.name LIKE ?")
        params.append(f"%{args.root}%")
    where = "WHERE " + " AND ".join(clauses)
    sql = f"""
        SELECT
            r.name AS root,
            t.full_name AS type,
            m.name AS member,
            rh.token,
            f.rel_path,
            rh.line
        FROM references_hint rh
        JOIN members m ON m.id = rh.member_id
        JOIN types t ON t.id = m.type_id
        JOIN source_files f ON f.id = rh.file_id
        JOIN source_roots r ON r.id = t.root_id
        {where}
        ORDER BY r.name, f.rel_path, rh.line
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def query_registrations(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    """Returns literal registration calls with both call-site and resolved type-declaration evidence when available."""

    clauses = []
    params = []
    if args.registration:
        clauses.append(
            "(reg.registration_key LIKE ? OR reg.type_name LIKE ? OR reg.method_name LIKE ?)"
        )
        value = f"%{args.registration}%"
        params.extend([value, value, value])
    if args.registration_kind:
        clauses.append("reg.registration_kind = ?")
        params.append(args.registration_kind)
    if args.registration_method:
        clauses.append("reg.method_name = ?")
        params.append(args.registration_method)
    if args.type:
        clauses.append("(reg.type_name LIKE ? OR resolved.full_name LIKE ?)")
        params.extend([f"%{args.type}%", f"%{args.type}%"])
    if args.root:
        clauses.append("root.name LIKE ?")
        params.append(f"%{args.root}%")
    where = "WHERE " + " AND ".join(clauses) if clauses else ""
    sql = f"""
        SELECT
            root.name AS root,
            reg.registration_kind AS kind,
            reg.registration_key AS key,
            reg.method_name AS method,
            reg.type_name,
            resolved.full_name AS resolved_type,
            owner.full_name AS containing_type,
            call_file.rel_path AS registration_path,
            reg.line,
            type_file.rel_path AS type_path,
            resolved.start_line AS type_line,
            reg.evidence
        FROM registrations reg
        JOIN source_files call_file ON call_file.id = reg.file_id
        JOIN source_roots root ON root.id = reg.root_id
        LEFT JOIN types owner ON owner.id = reg.containing_type_id
        LEFT JOIN types resolved ON resolved.id = reg.registered_type_id
        LEFT JOIN source_files type_file ON type_file.id = resolved.file_id
        {where}
        ORDER BY root.name, reg.registration_kind, reg.registration_key, call_file.rel_path, reg.line
        LIMIT ?
    """
    params.append(args.limit)
    try:
        return conn.execute(sql, params).fetchall()
    except sqlite3.OperationalError as exc:
        if "no such table: registrations" in str(exc):
            raise SystemExit("This code index predates registration extraction; rebuild it with build_code_index.py.")
        raise


def format_row(row: sqlite3.Row) -> str:
    return " | ".join(f"{key}={row[key]}" for key in row.keys())


if __name__ == "__main__":
    raise SystemExit(main())
