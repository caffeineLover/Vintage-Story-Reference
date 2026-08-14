#!/usr/bin/env python3
"""
Joins asset class identifiers to literal Vintage Story C# registration evidence.

The asset and code indexes remain independently rebuildable. This query-time join keeps that architecture intact while
giving consuming projects a direct path from a top-level JSON ``class`` or ``entityClass`` value to its registration
call and, when unambiguous, the registered C# type declaration.
"""

import argparse
import json
import sqlite3
from pathlib import Path


def main() -> int:
    """Validates both generated indexes, executes the bounded cross-reference query, and prints stable result fields."""

    parser = argparse.ArgumentParser(
        description="Join Vintage Story asset class values to literal C# registration calls."
    )
    parser.add_argument("--asset-db", required=True, help="Path to assets.sqlite")
    parser.add_argument("--code-db", required=True, help="Path to code.sqlite")
    parser.add_argument("--asset-class", help="Asset class or entityClass substring")
    parser.add_argument("--registration-key", help="Registration key substring")
    parser.add_argument("--type", help="Registered or resolved C# type substring")
    parser.add_argument("--kind", help="Exact relationship kind: block, item, entity, or block_entity")
    parser.add_argument("--category", help="Exact asset category, such as blocktypes or itemtypes")
    parser.add_argument("--domain", help="Exact asset domain")
    parser.add_argument("--root", help="Registration source-root substring")
    parser.add_argument("--format", choices=("text", "json"), default="text")
    parser.add_argument("--limit", type=int, default=25)
    args = parser.parse_args()

    asset_db = require_database(args.asset_db, "Asset")
    code_db = require_database(args.code_db, "Code")

    with sqlite3.connect(asset_db) as conn:
        conn.row_factory = sqlite3.Row
        conn.execute("ATTACH DATABASE ? AS code_index", (str(code_db),))
        try:
            rows = query_cross_references(conn, args)
        finally:
            conn.execute("DETACH DATABASE code_index")

    if args.format == "json":
        print(json.dumps([dict(row) for row in rows], indent=2))
    else:
        for row in rows:
            print(format_row(row))
    return 0


def require_database(value: str, label: str) -> Path:
    """Resolves a required SQLite input and fails with a source-specific message when it is unavailable."""

    path = Path(value).resolve()
    if not path.is_file():
        raise SystemExit(f"{label} index not found: {path}")
    return path


def query_cross_references(conn: sqlite3.Connection, args) -> list[sqlite3.Row]:
    """Returns only category-aware class relationships supported by both an asset fact and registration evidence."""

    clauses = [class_relationship_clause()]
    params = []
    if args.asset_class:
        clauses.append("fact.fact_value LIKE ?")
        params.append(f"%{args.asset_class}%")
    if args.registration_key:
        clauses.append("registration.registration_key LIKE ?")
        params.append(f"%{args.registration_key}%")
    if args.type:
        clauses.append("(registration.type_name LIKE ? OR resolved.full_name LIKE ?)")
        params.extend([f"%{args.type}%", f"%{args.type}%"])
    if args.kind:
        clauses.append("registration.registration_kind = ?")
        params.append(args.kind)
    if args.category:
        clauses.append("asset.category = ?")
        params.append(args.category)
    if args.domain:
        clauses.append("asset.domain = ?")
        params.append(args.domain)
    if args.root:
        clauses.append("root.name LIKE ?")
        params.append(f"%{args.root}%")

    where = "WHERE " + " AND ".join(clauses)
    sql = f"""
        SELECT
            fact.fact_value AS asset_class,
            registration.registration_kind AS kind,
            asset.domain,
            asset.category,
            asset.rel_path AS asset_path,
            fact.fact_key AS asset_fact,
            registration.registration_key,
            registration.method_name AS method,
            registration.type_name,
            resolved.full_name AS resolved_type,
            owner.full_name AS registration_owner,
            root.name AS registration_root,
            call_file.rel_path AS registration_path,
            registration.line AS registration_line,
            type_file.rel_path AS type_path,
            resolved.start_line AS type_line
        FROM json_assets fact
        JOIN assets asset ON asset.id = fact.asset_id
        JOIN code_index.registrations registration
            ON registration.registration_key = fact.fact_value COLLATE NOCASE
        JOIN code_index.source_files call_file ON call_file.id = registration.file_id
        JOIN code_index.source_roots root ON root.id = registration.root_id
        LEFT JOIN code_index.types owner ON owner.id = registration.containing_type_id
        LEFT JOIN code_index.types resolved ON resolved.id = registration.registered_type_id
        LEFT JOIN code_index.source_files type_file ON type_file.id = resolved.file_id
        {where}
        ORDER BY fact.fact_value, asset.rel_path, root.name, registration.line
        LIMIT ?
    """
    params.append(args.limit)
    try:
        return conn.execute(sql, params).fetchall()
    except sqlite3.OperationalError as exc:
        if "registrations" in str(exc):
            raise SystemExit("The code index predates registration extraction; rebuild it with build_code_index.py.")
        raise


def class_relationship_clause() -> str:
    """Constrains identical strings to the registry that Vintage Story uses for each top-level asset field."""

    return """
        (
            (fact.fact_key = 'class' AND fact.json_path = '$.class' AND asset.category = 'blocktypes'
                AND registration.registration_kind = 'block')
            OR
            (fact.fact_key = 'class' AND fact.json_path = '$.class' AND asset.category = 'itemtypes'
                AND registration.registration_kind = 'item')
            OR
            (fact.fact_key = 'class' AND fact.json_path = '$.class' AND asset.category = 'entities'
                AND registration.registration_kind = 'entity')
            OR
            (fact.fact_key = 'entityClass' AND fact.json_path = '$.entityClass' AND asset.category = 'blocktypes'
                AND registration.registration_kind = 'block_entity')
        )
    """


def format_row(row: sqlite3.Row) -> str:
    """Keeps human-readable output consistent with the existing source-specific query helpers."""

    return " | ".join(f"{key}={row[key]}" for key in row.keys())


if __name__ == "__main__":
    raise SystemExit(main())
