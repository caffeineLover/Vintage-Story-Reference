# Persistent Project Memory Instructions

You are working in a software project that may be continued by different coding agents across multiple sessions.

Your responsibility is not only to complete the current task, but also to maintain a durable project history in a file located under docs/ named:

`PROJECT_MEMORY.md`

This file must allow a future coding agent to understand what has already been tried, discovered, changed, rejected, and left unfinished, even if that agent has no access to previous conversations.

## Core rule

Before doing substantial work:

1. Look for `PROJECT_MEMORY.md` in docs/ under the repository root.
2. If it exists, read it completely before making changes.
3. If it does not exist, create it using the structure defined below.
4. Inspect the current repository state before trusting memory entries.
5. Treat the current code, configuration, tests, and version-control state as authoritative when they conflict with old notes.

Do not assume access to other repositories, sibling projects, prior chat sessions, or external context unless those are explicitly provided.

---

# What PROJECT_MEMORY.md is for

`PROJECT_MEMORY.md` is a persistent engineering notebook.

It is not merely:

- a task list;
- a changelog;
- a compressed conversation summary;
- a copy of the README;
- a replacement for source-code comments;
- a dump of every terminal command.

It should preserve information that helps another agent resume work intelligently.

This includes successful work, failed work, investigations, dead ends, design reasoning, API discoveries, environmental constraints, test results, and unresolved uncertainty.

Failed approaches are valuable and must be recorded when they could otherwise be repeated.

---

# Required file structure

Create or maintain `PROJECT_MEMORY.md` using this general structure:

```markdown
# Project Memory

Last updated: YYYY-MM-DD

## Project overview

A concise description of the project, its purpose, and its current development status.

## Current state

Describe what currently works, what is partially implemented, and what is known to be broken.

This section should reflect the repository as it exists now, not merely the intent of past sessions.

## Active work

Describe the work currently in progress.

Include:

- the immediate goal;
- relevant files or components;
- partially completed implementation;
- the most useful next action.

## Durable technical knowledge

Record stable facts discovered about the project.

Examples:

- important APIs and their actual behavior;
- version-specific behavior;
- required initialization order;
- asset or configuration paths;
- naming conventions;
- serialization formats;
- build requirements;
- runtime constraints;
- behavior that was verified experimentally.

Only promote information here when it is reasonably well established.

## Architecture and design decisions

For each significant decision, record:

- what was decided;
- why it was chosen;
- alternatives considered;
- tradeoffs;
- any conditions under which the decision should be revisited.

## External interfaces and integrations

Document contracts with systems outside this repository.

Examples:

- file formats;
- JSON schemas;
- API expectations;
- asset identifiers;
- optional integrations;
- dependency boundaries;
- compatibility assumptions.

Do not assume another repository is available. Describe the contract sufficiently within this project.

## Known failed approaches

Record approaches that were attempted or seriously considered but did not work.

For each one, include:

- what was attempted;
- why it seemed reasonable;
- what happened;
- relevant errors or observations;
- why it was abandoned;
- whether it may become viable under different conditions.

Do not remove failed approaches merely because a working solution was later found.

## Bugs, risks, and limitations

Record known bugs, likely failure modes, compatibility risks, performance concerns, and incomplete behavior.

Distinguish confirmed bugs from suspected risks.

## Open questions

List unresolved technical or design questions.

For each question, include any evidence already gathered so future agents do not restart the investigation from zero.

## Testing and verification

Describe:

- available test commands;
- manual test procedures;
- known-good environments;
- important fixtures or test data;
- tests that currently fail;
- tests that have not yet been performed.

## Useful commands

Record commands that are genuinely useful for future work.

Examples:

- build commands;
- test commands;
- formatting commands;
- packaging commands;
- launch commands;
- diagnostic commands.

Do not include transient commands that have no future value.

## Important files and code paths

List significant files, classes, methods, asset paths, or configuration files and explain their purpose.

## Session history

Append dated session entries below this heading.

Do not delete or overwrite previous entries.