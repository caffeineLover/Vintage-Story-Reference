#!/usr/bin/env python3
"""
Queries a tracked Vintage Story reference-mod manifest.

The manifest records curated provenance and compatibility facts that cannot be
reliably reconstructed from an asset or code index. This helper keeps those
facts searchable without introducing another generated SQLite database.
"""

import argparse
import json
from pathlib import Path
from typing import Any


def main() -> int:
    """Loads one manifest, applies the requested filters, and prints compact or full entries."""

    parser = argparse.ArgumentParser(description="Query a Vintage Story reference-mod manifest.")
    parser.add_argument("--manifest", required=True, help="Path to mods.manifest.json")
    parser.add_argument("--list", action="store_true", help="List matching entries; this is the default action.")
    parser.add_argument("--mod-id", help="Filter by exact mod id")
    parser.add_argument("--search", help="Search compact mod metadata and provenance fields")
    parser.add_argument("--compatibility", help="Filter by compatibility status, such as exact or same-line-fallback")
    parser.add_argument("--package-type", help="Filter by package type, such as code, content, or externaltool")
    parser.add_argument("--has-assets", type=parse_boolean, metavar="{yes,no}")
    parser.add_argument("--has-code", type=parse_boolean, metavar="{yes,no}")
    parser.add_argument("--has-native-binaries", type=parse_boolean, metavar="{yes,no}")
    parser.add_argument("--has-license-file", type=parse_boolean, metavar="{yes,no}")
    parser.add_argument("--format", choices=("text", "json"), default="text")
    parser.add_argument("--limit", type=positive_integer, default=25)
    args = parser.parse_args()

    manifest_path = Path(args.manifest)
    manifest = load_manifest(manifest_path)
    matches = [mod for mod in manifest["mods"] if matches_filters(mod, args)]
    matches.sort(key=lambda mod: str(mod.get("mod_id", "")).casefold())
    matches = matches[: args.limit]

    if args.format == "json":
        print(json.dumps(matches, ensure_ascii=False, indent=2))
    else:
        for mod in matches:
            print(format_row(mod))

    return 0



def parse_boolean(value: str) -> bool:
    """Accepts agent-friendly yes/no values while retaining common Boolean spellings."""

    normalized = value.casefold()
    if normalized in {"yes", "true", "1"}:
        return True
    if normalized in {"no", "false", "0"}:
        return False
    raise argparse.ArgumentTypeError("expected yes or no")



def positive_integer(value: str) -> int:
    """Rejects zero and negative result limits before querying the manifest."""

    parsed = int(value)
    if parsed < 1:
        raise argparse.ArgumentTypeError("expected an integer greater than zero")
    return parsed



def load_manifest(path: Path) -> dict[str, Any]:
    """Reads the tracked JSON source and verifies the minimum top-level contract needed by the query tool."""

    if not path.is_file():
        raise SystemExit(f"Manifest not found: {path}")

    try:
        manifest = json.loads(path.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        raise SystemExit(f"Could not read mod manifest {path}: {exc}") from exc

    if not isinstance(manifest, dict) or not isinstance(manifest.get("mods"), list):
        raise SystemExit(f"Invalid mod manifest: {path} must contain a top-level mods array.")

    return manifest



def matches_filters(mod: dict[str, Any], args: argparse.Namespace) -> bool:
    """Applies exact categorical filters and a compact case-insensitive metadata search to one entry."""

    if args.mod_id and str(mod.get("mod_id", "")).casefold() != args.mod_id.casefold():
        return False

    compatibility = mod.get("compatibility", {})
    if args.compatibility and str(compatibility.get("status", "")).casefold() != args.compatibility.casefold():
        return False

    if args.package_type and str(mod.get("package_type", "")).casefold() != args.package_type.casefold():
        return False

    reference_content = mod.get("reference_content", {})
    license_metadata = mod.get("license", {})
    boolean_filters = (
        (args.has_assets, reference_content.get("has_assets")),
        (args.has_code, reference_content.get("has_decompiled_code")),
        (args.has_native_binaries, reference_content.get("has_native_binaries")),
        (args.has_license_file, license_metadata.get("file_present")),
    )
    if any(expected is not None and actual is not expected for expected, actual in boolean_filters):
        return False

    if args.search:
        query = args.search.casefold()
        if query not in searchable_text(mod).casefold():
            return False

    return True



def searchable_text(mod: dict[str, Any]) -> str:
    """Builds bounded search text from curated fields without serializing or searching retained mod contents."""

    compatibility = mod.get("compatibility", {})
    sources = mod.get("sources", {})
    reference_content = mod.get("reference_content", {})
    license_metadata = mod.get("license", {})
    values = [
        mod.get("mod_id"),
        mod.get("name"),
        mod.get("version"),
        mod.get("package_type"),
        mod.get("folder"),
        compatibility.get("status"),
        compatibility.get("target_game_version"),
        *compatibility.get("release_tags", []),
        *sources.values(),
        *reference_content.get("code_roots", []),
        license_metadata.get("status"),
    ]
    return " ".join(str(value) for value in values if value is not None)



def format_row(mod: dict[str, Any]) -> str:
    """Formats the fields most useful for selecting a reference mod and scoping later asset/code queries."""

    compatibility = mod.get("compatibility", {})
    sources = mod.get("sources", {})
    reference_content = mod.get("reference_content", {})
    license_metadata = mod.get("license", {})
    code_roots = ",".join(reference_content.get("code_roots", [])) or "(none)"
    source = sources.get("mod_db_page") or sources.get("source_repository") or "(none)"

    fields = {
        "mod_id": mod.get("mod_id", ""),
        "name": mod.get("name", ""),
        "version": mod.get("version", ""),
        "type": mod.get("package_type", ""),
        "compatibility": compatibility.get("status", ""),
        "folder": mod.get("folder", ""),
        "assets": format_boolean(reference_content.get("has_assets")),
        "code": format_boolean(reference_content.get("has_decompiled_code")),
        "native_binaries": format_boolean(reference_content.get("has_native_binaries")),
        "license_file": format_boolean(license_metadata.get("file_present")),
        "code_roots": code_roots,
        "source": source,
    }
    return " | ".join(f"{key}={value}" for key, value in fields.items())



def format_boolean(value: Any) -> str:
    """Preserves unknown metadata instead of silently presenting it as a negative result."""

    if value is True:
        return "yes"
    if value is False:
        return "no"
    return "unknown"



if __name__ == "__main__":
    raise SystemExit(main())
