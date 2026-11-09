import importlib.util
import sys
import textwrap
import unittest
from pathlib import Path


MODULE_PATH = Path(__file__).parents[1] / "tools" / "code-index" / "build_code_index.py"
MODULE_SPEC = importlib.util.spec_from_file_location("build_code_index", MODULE_PATH)
if MODULE_SPEC is None or MODULE_SPEC.loader is None:
    raise RuntimeError(f"Unable to load {MODULE_PATH}")

build_code_index = importlib.util.module_from_spec(MODULE_SPEC)
sys.modules[MODULE_SPEC.name] = build_code_index
MODULE_SPEC.loader.exec_module(build_code_index)


class ParseMembersTests(unittest.TestCase):
    def test_parse_types_uses_declared_delegate_name_and_single_line_bounds(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public delegate bool OnTryTextChangeDelegate(List<string> lines);

                private struct TextSelection
                {
                    internal TextSelection(int start, int end)
                    {
                    }
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)

        self.assertEqual(
            [
                ("class", "ExampleSystem"),
                ("delegate", "OnTryTextChangeDelegate"),
                ("struct", "TextSelection"),
            ],
            [(declaration.kind, declaration.name) for declaration in declarations],
        )
        delegate = declarations[1]
        self.assertEqual(delegate.start_index, delegate.end_index)

    def test_parse_members_skips_multiline_delegate_parameters(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public delegate bool OnTryTextChangeDelegate(
                    List<string> lines,
                    string value
                );

                public void StartServerSide()
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        delegate = next(declaration for declaration in declarations if declaration.kind == "delegate")
        self.assertGreater(delegate.end_index, delegate.start_index)

        example = next(declaration for declaration in declarations if declaration.name == "ExampleSystem")
        members = build_code_index.parse_members(source, example)
        self.assertEqual(
            [("method", "StartServerSide")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_accepts_allman_style_method_and_constructor(self) -> None:
        source = textwrap.dedent(
            """
            namespace Example;

            public class ExampleSystem
            {
                public string Name { get; set; }

                public ExampleSystem()
                {
                }

                public virtual void StartServerSide(ICoreServerAPI api)
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        self.assertEqual(1, len(declarations))

        members = build_code_index.parse_members(source, declarations[0])
        self.assertEqual(
            [
                ("property", "Name"),
                ("constructor", "ExampleSystem"),
                ("method", "StartServerSide"),
            ],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_keeps_consecutive_single_line_bodies_separate(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public ExampleSystem() { }
                public void First() { }
                public void Second() { }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            [("constructor", "ExampleSystem"), ("method", "First"), ("method", "Second")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_types_keeps_consecutive_single_line_bodies_separate(self) -> None:
        source = textwrap.dedent(
            """
            public class First { }
            public class Second { }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)

        self.assertEqual(
            [("First", 2, 2), ("Second", 3, 3)],
            [(declaration.name, declaration.start_line, declaration.end_line) for declaration in declarations],
        )

    def test_parse_members_accepts_constructor_initializers(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public ExampleSystem() : base()
                {
                }

                public ExampleSystem(int value)
                    : this()
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            ["ExampleSystem", "ExampleSystem"],
            [member.name for member in members if member.kind == "constructor"],
        )

    def test_constructor_object_initializer_does_not_truncate_body_span(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public ExampleSystem() : base(new Options { Enabled = true })
                {
                    RegisterSomething();
                }

                public void StartServerSide() { }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])
        constructor = next(member for member in members if member.kind == "constructor")

        self.assertEqual(7, constructor.end_line)
        self.assertEqual(
            [("constructor", "ExampleSystem"), ("method", "StartServerSide")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_rejects_declaration_shaped_linq_clause(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                private readonly IEnumerable<int> values =
                    from item in GetItems()
                    select item;
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            [("field", "values")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_rejects_typed_and_escaped_linq_range_variables(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                private readonly IEnumerable<Customer> typedValues = (
                    from Customer item in GetItems()
                    where item.IsActive
                    select item
                );

                private readonly IEnumerable<Customer> escapedValues = (
                    from @item in GetItems()
                    select item
                );

                public void StartServerSide()
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            [("method", "StartServerSide")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_rejects_async_lambda_continuation_as_method(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                private readonly Func<Task<int>> getValue = async () =>
                    await GetValue()
                    ;

                private readonly Func<Task<int>> getOtherValue = async () =>
                    await GetOtherValue();

                public void StartServerSide()
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            [("method", "StartServerSide")],
            [(member.kind, member.name) for member in members],
        )

    def test_parse_members_accepts_contextual_keywords_as_identifiers(self) -> None:
        source = textwrap.dedent(
            """
            public interface ExampleSystem
            {
                Result from();
                from Select();
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            [("Result", "from"), ("from", "Select")],
            [(member.return_type, member.name) for member in members],
        )

    def test_parse_members_ignores_braces_in_character_literals(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem
            {
                public char ClosingBrace()
                {
                    return '}';
                }

                public void StartServerSide()
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        members = build_code_index.parse_members(source, declarations[0])

        self.assertEqual(
            ["ClosingBrace", "StartServerSide"],
            [member.name for member in members if member.kind == "method"],
        )

    def test_parse_members_ignores_apostrophes_and_braces_in_block_comments(self) -> None:
        source = textwrap.dedent(
            """
            public class ExampleSystem /* author's note */ {
                public void StartServerSide()
                /* maintainer's comment with misleading braces:
                   { }
                */
                {
                }
            }
            """
        ).splitlines()

        declarations = build_code_index.parse_types(source)
        self.assertEqual(1, len(declarations))

        members = build_code_index.parse_members(source, declarations[0])
        self.assertEqual(
            ["StartServerSide"],
            [member.name for member in members if member.kind == "method"],
        )


if __name__ == "__main__":
    unittest.main()
