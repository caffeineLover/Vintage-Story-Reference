#!/usr/bin/env python3
"""
Unified query entry point for the Vintage Story Reference repository.

The individual asset, code, and web query helpers remain the source-specific
interfaces. This wrapper reads the repository catalogs, resolves the requested
game version and index database, and then delegates to the appropriate helper.
It gives other agents one stable command shape while keeping the specialized
query behavior in the existing tools.
"""

import argparse
import json
import subprocess
import sys
from pathlib import Path


INDEX_ALIASES = {
    "asset": "asset",
    "assets": "asset",
    "code": "code",
    "web": "web",
}


def main() -> int:
    parser = argparse.ArgumentParser(description="Query Vintage Story Reference indexes through one wrapper.")
    parser.add_argument("--catalog", help="Path to reference.catalog.json; defaults to the repository root catalog.")
    parser.add_argument("--version", help="Vintage Story version to query. Defaults to the catalog default.")
    parser.add_argument("--list-versions", action="store_true", help="List game versions known to the root catalog.")
    parser.add_argument("--list-indexes", action="store_true", help="List indexes available for the selected version.")
    parser.add_argument("index", nargs="?", help="Index to query: asset, code, or web.")
    parser.add_argument("query_args", nargs=argparse.REMAINDER, help="Arguments passed through to the selected query helper.")
    args = parser.parse_args()

    repo_root = Path(__file__).resolve().parents[1]
    catalog_path = Path(args.catalog).resolve() if args.catalog else repo_root / "reference.catalog.json"
    root_catalog = load_json(catalog_path)

    if args.list_versions:
        list_versions(root_catalog)
        return 0

    version = args.version or root_catalog.get("default_game_version")
    if not version:
        raise SystemExit("No version supplied and the root catalog has no default_game_version.")

    version_catalog = load_version_catalog(repo_root, root_catalog, version)

    if args.list_indexes:
        list_indexes(version, version_catalog)
        return 0

    if not args.index:
        raise SystemExit("Specify an index to query, or use --list-versions / --list-indexes.")

    index_name = INDEX_ALIASES.get(args.index.lower())
    if not index_name:
        valid = ", ".join(sorted(INDEX_ALIASES))
        raise SystemExit(f"Unknown index '{args.index}'. Valid values: {valid}")

    return run_query_helper(repo_root, version_catalog, index_name, args.query_args)



def load_json(path: Path) -> dict:
    if not path.is_file():
        raise SystemExit(f"Catalog not found: {path}")
    return json.loads(path.read_text(encoding="utf-8"))



def load_version_catalog(repo_root: Path, root_catalog: dict, version: str) -> dict:
    versions = root_catalog.get("versions", {})
    version_entry = versions.get(version)
    if not version_entry:
        known = ", ".join(sorted(versions)) or "(none)"
        raise SystemExit(f"Version '{version}' is not in the root catalog. Known versions: {known}")

    catalog_rel_path = version_entry.get("catalog")
    if not catalog_rel_path:
        raise SystemExit(f"Version '{version}' has no catalog path in reference.catalog.json.")

    return load_json(repo_root / catalog_rel_path)



def list_versions(root_catalog: dict) -> None:
    default_version = root_catalog.get("default_game_version")
    for version in sorted(root_catalog.get("versions", {})):
        marker = " (default)" if version == default_version else ""
        print(f"{version}{marker}")



def list_indexes(version: str, version_catalog: dict) -> None:
    for name, index in sorted(version_catalog.get("indexes", {}).items()):
        database = index.get("database", "")
        uses = ", ".join(index.get("use_for", []))
        print(f"{version} | {name} | database={database} | use_for={uses}")



def run_query_helper(repo_root: Path, version_catalog: dict, index_name: str, query_args: list[str]) -> int:
    indexes = version_catalog.get("indexes", {})
    index = indexes.get(index_name)
    if not index:
        raise SystemExit(f"Index '{index_name}' is not configured for version {version_catalog.get('game_version')}.")

    script = repo_root / index["query_script"]
    database = repo_root / index["database"]
    if not script.is_file():
        raise SystemExit(f"Query helper not found: {script}")
    if not database.is_file():
        raise SystemExit(f"Index database not found: {database}")

    command = [
        sys.executable,
        "-B",
        str(script),
        "--db",
        str(database),
        *query_args,
    ]
    result = subprocess.run(command, cwd=repo_root)
    return int(result.returncode)


if __name__ == "__main__":
    raise SystemExit(main())
