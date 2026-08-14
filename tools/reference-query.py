#!/usr/bin/env python3
"""
Unified query entry point for the Vintage Story Reference repository.

The individual asset, code, class-cross-reference, mod-manifest, and web query helpers remain the
source-specific interfaces. This wrapper reads the repository catalogs,
resolves the requested game version and data source, and then delegates to the
appropriate helper. It gives other agents one stable command shape while
keeping specialized query behavior in the existing tools.
"""

import argparse
import json
import subprocess
import sys
from pathlib import Path


QUERY_ALIASES = {
    "asset": ("indexes", "asset"),
    "assets": ("indexes", "asset"),
    "code": ("indexes", "code"),
    "class": ("cross_references", "classes"),
    "classes": ("cross_references", "classes"),
    "mod": ("manifests", "mods"),
    "mods": ("manifests", "mods"),
    "web": ("indexes", "web"),
    "xref": ("cross_references", "classes"),
}


def main() -> int:
    parser = argparse.ArgumentParser(description="Query Vintage Story Reference sources through one wrapper.")
    parser.add_argument("--catalog", help="Path to reference.catalog.json; defaults to the repository root catalog.")
    parser.add_argument("--version", help="Vintage Story version to query. Defaults to the catalog default.")
    parser.add_argument("--list-versions", action="store_true", help="List game versions known to the root catalog.")
    parser.add_argument("--list-indexes", action="store_true", help="List indexes available for the selected version.")
    parser.add_argument(
        "--list-cross-references",
        action="store_true",
        help="List query-time cross-references available for the selected version.",
    )
    parser.add_argument(
        "--list-manifests",
        action="store_true",
        help="List curated manifests available for the selected version.",
    )
    parser.add_argument(
        "source",
        nargs="?",
        help="Reference source to query: asset, code, class/xref, mod, or web.",
    )
    parser.add_argument(
        "query_args",
        nargs=argparse.REMAINDER,
        help="Arguments passed through to the selected query helper.",
    )
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

    if args.list_cross_references:
        list_cross_references(version, version_catalog)
        return 0

    if args.list_manifests:
        list_manifests(version, version_catalog)
        return 0

    if not args.source:
        raise SystemExit("Specify a source to query, or use a list option.")

    query_target = QUERY_ALIASES.get(args.source.lower())
    if not query_target:
        valid = ", ".join(sorted(QUERY_ALIASES))
        raise SystemExit(f"Unknown query source '{args.source}'. Valid values: {valid}")

    collection_name, source_name = query_target
    return run_query_helper(repo_root, version_catalog, collection_name, source_name, args.query_args)



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


def list_cross_references(version: str, version_catalog: dict) -> None:
    """Prints query-time relationships that combine two or more cataloged reference sources."""

    for name, cross_reference in sorted(version_catalog.get("cross_references", {}).items()):
        inputs = ", ".join(cross_reference.get("inputs", {}).values())
        uses = ", ".join(cross_reference.get("use_for", []))
        print(f"{version} | {name} | inputs={inputs} | use_for={uses}")



def list_manifests(version: str, version_catalog: dict) -> None:
    """Prints tracked manifest sources separately from rebuildable SQLite indexes."""

    for name, manifest in sorted(version_catalog.get("manifests", {}).items()):
        path = manifest.get("manifest", "")
        uses = ", ".join(manifest.get("use_for", []))
        print(f"{version} | {name} | manifest={path} | use_for={uses}")



def run_query_helper(
    repo_root: Path,
    version_catalog: dict,
    collection_name: str,
    source_name: str,
    query_args: list[str],
) -> int:
    """Delegates a cataloged query source to its specialized helper without duplicating query behavior."""

    sources = version_catalog.get(collection_name, {})
    source = sources.get(source_name)
    if not source:
        raise SystemExit(
            f"Query source '{source_name}' is not configured for version {version_catalog.get('game_version')}."
        )

    script = repo_root / source["query_script"]
    if not script.is_file():
        raise SystemExit(f"Query helper not found: {script}")

    input_args = resolve_query_inputs(repo_root, source)
    command = [sys.executable, "-B", str(script), *input_args, *query_args]
    result = subprocess.run(command, cwd=repo_root)
    return int(result.returncode)



def resolve_query_inputs(repo_root: Path, source: dict) -> list[str]:
    """Maps one or more cataloged inputs to the flags expected by a specialized query helper."""

    configured_inputs = source.get("inputs", {})
    if configured_inputs:
        result = []
        for flag, relative_path in configured_inputs.items():
            input_path = repo_root / relative_path
            if not input_path.is_file():
                raise SystemExit(f"Query input not found: {input_path}")
            result.extend([flag, str(input_path)])
        return result

    for key, flag in (("database", "--db"), ("manifest", "--manifest")):
        relative_path = source.get(key)
        if not relative_path:
            continue

        input_path = repo_root / relative_path
        if not input_path.is_file():
            raise SystemExit(f"Query input not found: {input_path}")
        return [flag, str(input_path)]

    raise SystemExit("Query source must configure inputs, a database, or a manifest path.")



if __name__ == "__main__":
    raise SystemExit(main())
