#!/usr/bin/env python3
import argparse
import sqlite3
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser(description="Query the Vintage Story Reference asset index.")
    parser.add_argument("--db", required=True, help="Path to assets.sqlite")
    parser.add_argument("--search", help="FTS search text")
    parser.add_argument("--category", help="Filter by category")
    parser.add_argument("--domain", help="Filter by domain")
    parser.add_argument("--fact-key", help="Filter json_assets by fact key")
    parser.add_argument("--fact-value", help="Filter json_assets by fact value substring")
    parser.add_argument("--limit", type=int, default=25)
    args = parser.parse_args()

    db_path = Path(args.db)
    if not db_path.is_file():
        raise SystemExit(f"Index not found: {db_path}")

    with sqlite3.connect(db_path) as conn:
        conn.row_factory = sqlite3.Row
        if args.fact_key or args.fact_value:
            rows = query_facts(conn, args)
        else:
            rows = query_assets(conn, args)

    for row in rows:
        print(format_row(row))
    return 0


def query_assets(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = []
    params = []
    join = ""
    if args.search:
        join = "JOIN asset_text t ON t.rowid = a.id"
        clauses.append("asset_text MATCH ?")
        params.append(args.search)
    if args.category:
        clauses.append("a.category = ?")
        params.append(args.category)
    if args.domain:
        clauses.append("a.domain = ?")
        params.append(args.domain)
    where = "WHERE " + " AND ".join(clauses) if clauses else ""
    sql = f"""
        SELECT a.rel_path, a.domain, a.category, a.extension, a.size, a.parse_status
        FROM assets a
        {join}
        {where}
        ORDER BY a.rel_path
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def query_facts(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    clauses = []
    params = []
    if args.fact_key:
        clauses.append("j.fact_key = ?")
        params.append(args.fact_key)
    if args.fact_value:
        clauses.append("j.fact_value LIKE ?")
        params.append(f"%{args.fact_value}%")
    where = "WHERE " + " AND ".join(clauses) if clauses else ""
    sql = f"""
        SELECT a.rel_path, a.domain, a.category, j.fact_key, j.fact_value, j.json_path
        FROM json_assets j
        JOIN assets a ON a.id = j.asset_id
        {where}
        ORDER BY a.rel_path
        LIMIT ?
    """
    params.append(args.limit)
    return conn.execute(sql, params).fetchall()


def format_row(row: sqlite3.Row) -> str:
    keys = row.keys()
    return " | ".join(f"{key}={row[key]}" for key in keys)


if __name__ == "__main__":
    raise SystemExit(main())
