using System;
using System.Collections.Generic;
using System.Text;
using Cairo;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

public abstract class GuiElementEditableTextBase : GuiElementTextBase
{
	public delegate bool OnTryTextChangeDelegate(List<string> lines);

	private struct TextSelection
	{
		internal int Start;

		internal int End;

		internal TextSelection(int start, int end)
		{
			Start = start;
			End = end;
		}
	}

	private enum CopyCutMode
	{
		Copy,
		Cut
	}

	private struct SelectedTextPos
	{
		internal int X;

		internal int Y;

		internal SelectedTextPos(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	private const int DoubleClickMilliseconds = 400;

	internal float[] caretColor = new float[4] { 1f, 1f, 1f, 1f };

	internal bool hideCharacters;

	internal bool multilineMode;

	internal int maxlines = 99999;

	internal int maxlength = -1;

	internal double caretX;

	internal double caretY;

	internal double topPadding;

	internal double leftPadding = 3.0;

	internal double rightSpacing;

	internal double bottomSpacing;

	private int? selectedTextStart;

	private long lastClickTime;

	private int lastClickCursor;

	private bool handlingOnKeyEvent;

	private bool mouseDown;

	internal LoadedTexture caretTexture;

	internal LoadedTexture textTexture;

	private int selectionTextureId;

	public Action<int, int> OnCaretPositionChanged;

	public Action<string> OnTextChanged;

	public OnTryTextChangeDelegate OnTryTextChangeText;

	public Action<double, double> OnCursorMoved;

	internal Action OnFocused;

	internal Action OnLostFocus;

	/// <summary>
	/// Called when a keyboard key was pressed, received and handled
	/// </summary>
	public Action OnKeyPressed;

	internal long caretBlinkMilliseconds;

	internal bool caretDisplayed;

	internal double caretHeight;

	internal double renderLeftOffset;

	internal Vec2i textSize = new Vec2i();

	protected List<string> lines;

	/// <summary>
	/// Contains the same as Lines, but may momentarily have different values when an edit is being made
	/// </summary>
	protected List<string> linesStaging;

	public bool WordWrap = true;

	protected int pcaretPosLine;

	protected int pcaretPosInLine;

	public int TextLengthWithoutLineBreaks
	{
		get
		{
			int length = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				length += lines[i].Length;
			}
			return length;
		}
	}

	public int CaretPosWithoutLineBreaks
	{
		get
		{
			int pos = 0;
			for (int i = 0; i < CaretPosLine; i++)
			{
				pos += lines[i].Length;
			}
			return pos + CaretPosInLine;
		}
		set
		{
			int sum = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				int len = lines[i].Length;
				if (sum + len > value)
				{
					SetCaretPos(value - sum, i);
					return;
				}
				sum += len;
			}
			if (!multilineMode)
			{
				SetCaretPos(sum);
			}
			else
			{
				SetCaretPos(sum, lines.Count);
			}
		}
	}

	public int CaretPosLine
	{
		get
		{
			return pcaretPosLine;
		}
		set
		{
			pcaretPosLine = value;
		}
	}

	public int CaretPosInLine
	{
		get
		{
			return pcaretPosInLine;
		}
		set
		{
			if (value > lines[CaretPosLine].Length)
			{
				throw new IndexOutOfRangeException("Caret @" + value + ", cannot beyond current line length of " + pcaretPosInLine);
			}
			pcaretPosInLine = value;
		}
	}

	public override bool Focusable => enabled;

	public List<string> GetLines()
	{
		return new List<string>(lines);
	}

	/// <summary>
	/// Initializes the text component.
	/// </summary>
	/// <param name="capi">The Client API</param>
	/// <param name="font">The font of the text.</param>
	/// <param name="bounds">The bounds of the component.</param>
	public GuiElementEditableTextBase(ICoreClientAPI capi, CairoFont font, ElementBounds bounds)
		: base(capi, "", font, bounds)
	{
		caretTexture = new LoadedTexture(capi);
		textTexture = new LoadedTexture(capi);
		selectionTextureId = GenerateSelectionTexture(capi);
		lines = new List<string> { "" };
		linesStaging = new List<string> { "" };
	}

	public override void OnFocusGained()
	{
		base.OnFocusGained();
		OnFocused?.Invoke();
	}

	public override void OnFocusLost()
	{
		base.OnFocusLost();
		selectedTextStart = null;
		OnLostFocus?.Invoke();
	}

	/// <summary>
	/// Sets the position of the cursor at a given point.
	/// </summary>
	/// <param name="x">X position of the cursor.</param>
	/// <param name="y">Y position of the cursor.</param>
	public void SetCaretPos(double x, double y)
	{
		CaretPosLine = 0;
		ImageSurface surface = new ImageSurface(Format.Argb32, 1, 1);
		Context ctx = genContext(surface);
		Font.SetupContext(ctx);
		if (multilineMode)
		{
			double lineY = y / ctx.FontExtents.Height;
			if (lineY > (double)lines.Count)
			{
				CaretPosLine = lines.Count - 1;
				CaretPosInLine = lines[CaretPosLine].Length;
				ctx.Dispose();
				surface.Dispose();
				return;
			}
			CaretPosLine = Math.Max(0, (int)lineY);
		}
		CaretPosLine = Math.Clamp(CaretPosLine, 0, lines.Count - 1);
		string line = lines[CaretPosLine].TrimEnd(new char[2] { '\r', '\n' });
		CaretPosInLine = line.Length;
		for (int i = 0; i < line.Length; i++)
		{
			double posx = ctx.TextExtents(line.Substring(0, i + 1)).XAdvance;
			if (x - posx <= 0.0)
			{
				CaretPosInLine = i;
				break;
			}
		}
		ctx.Dispose();
		surface.Dispose();
		SetCaretPos(CaretPosInLine, CaretPosLine);
	}

	/// <summary>
	/// Sets the position of the cursor to a specific character.
	/// </summary>
	/// <param name="posInLine">The position in the line.</param>
	/// <param name="posLine">The line of the text.</param>
	public void SetCaretPos(int posInLine, int posLine = 0)
	{
		caretBlinkMilliseconds = api.ElapsedMilliseconds;
		caretDisplayed = true;
		CaretPosLine = GameMath.Clamp(posLine, 0, lines.Count - 1);
		CaretPosInLine = GameMath.Clamp(posInLine, 0, lines[CaretPosLine].TrimEnd(new char[2] { '\r', '\n' }).Length);
		if (multilineMode)
		{
			caretX = Font.GetTextExtents(lines[CaretPosLine].Substring(0, CaretPosInLine)).XAdvance;
			caretY = Font.GetFontExtents().Height * (double)CaretPosLine;
		}
		else
		{
			string displayedText = lines[0];
			if (hideCharacters)
			{
				displayedText = new StringBuilder(lines[0]).Insert(0, "•", displayedText.Length).ToString();
			}
			caretX = Font.GetTextExtents(displayedText.Substring(0, CaretPosInLine)).XAdvance;
			caretY = 0.0;
		}
		OnCursorMoved?.Invoke(caretX, caretY);
		renderLeftOffset = Math.Max(0.0, caretX - Bounds.InnerWidth + rightSpacing);
		OnCaretPositionChanged?.Invoke(posLine, posInLine);
	}

	/// <summary>
	/// Sets a numerical value to the text, appending it to the end of the text.
	/// </summary>
	/// <param name="value">The value to add to the text.</param>
	public void SetValue(float value)
	{
		SetValue(value.ToString(GlobalConstants.DefaultCultureInfo));
	}

	/// <summary>
	/// Sets a numerical value to the text, appending it to the end of the text.
	/// </summary>
	/// <param name="value">The value to add to the text.</param>
	public void SetValue(double value)
	{
		SetValue(value.ToString(GlobalConstants.DefaultCultureInfo));
	}

	/// <summary>
	/// Sets given text, sets the cursor to the end of the text
	/// </summary>
	/// <param name="text"></param>
	/// <param name="setCaretPosToEnd"></param>
	public void SetValue(string text, bool setCaretPosToEnd = true)
	{
		LoadValue(Lineize(text));
		if (setCaretPosToEnd)
		{
			int endPos = lines[lines.Count - 1].Length;
			SetCaretPos(endPos, lines.Count - 1);
		}
	}

	/// <summary>
	/// Sets given texts, leaves cursor position unchanged unless it's now invalid
	/// </summary>
	/// <param name="newLines"></param>
	public virtual void LoadValue(List<string> newLines)
	{
		OnTryTextChangeDelegate onTryTextChangeText = OnTryTextChangeText;
		if ((onTryTextChangeText != null && !onTryTextChangeText(newLines)) || (newLines.Count > maxlines && newLines.Count >= lines.Count))
		{
			linesStaging = new List<string>(lines);
			return;
		}
		lines = new List<string>(newLines);
		linesStaging = new List<string>(lines);
		CaretPosLine = Math.Clamp(CaretPosLine, 0, lines.Count - 1);
		CaretPosInLine = Math.Clamp(CaretPosInLine, 0, lines[CaretPosLine].Length);
		TextChanged();
	}

	public List<string> Lineize(string text)
	{
		if (text == null || maxlength == 0)
		{
			text = "";
		}
		if (maxlength > 0 && text.Length > maxlength)
		{
			text = text.Substring(0, maxlength);
		}
		List<string> lines = new List<string>();
		text = text.Replace("\r\n", "\n").Replace('\r', '\n');
		if (multilineMode)
		{
			double boxWidth = Bounds.InnerWidth - 2.0 * Bounds.absPaddingX;
			if (!WordWrap)
			{
				boxWidth = 999999.0;
			}
			TextLine[] array = textUtil.Lineize(Font, text, boxWidth, EnumLinebreakBehavior.Default, keepLinebreakChar: true);
			foreach (TextLine val in array)
			{
				lines.Add(val.Text);
			}
			if (lines.Count == 0)
			{
				lines.Add("");
			}
		}
		else
		{
			lines.Add(text);
		}
		return lines;
	}

	internal virtual void TextChanged()
	{
		OnTextChanged?.Invoke(string.Join("", lines));
		if (!handlingOnKeyEvent)
		{
			selectedTextStart = null;
		}
		RecomposeText();
	}

	internal virtual void RecomposeText()
	{
		Bounds.CalcWorldBounds();
		string displayedText = null;
		if (multilineMode)
		{
			textSize.X = (int)(Bounds.OuterWidth - rightSpacing);
			textSize.Y = (int)(Bounds.OuterHeight - bottomSpacing);
		}
		else
		{
			displayedText = lines[0];
			if (hideCharacters)
			{
				displayedText = new StringBuilder(displayedText.Length).Insert(0, "•", displayedText.Length).ToString();
			}
			textSize.X = (int)Math.Max(Bounds.InnerWidth - rightSpacing, Font.GetTextExtents(displayedText).Width);
			textSize.Y = (int)(Bounds.InnerHeight - bottomSpacing);
		}
		ImageSurface surface = new ImageSurface(Format.Argb32, textSize.X, textSize.Y);
		Context ctx = genContext(surface);
		Font.SetupContext(ctx);
		double fontHeight = ctx.FontExtents.Height;
		if (multilineMode)
		{
			double width = Bounds.InnerWidth - 2.0 * Bounds.absPaddingX - rightSpacing;
			TextLine[] textlines = new TextLine[lines.Count];
			for (int i = 0; i < textlines.Length; i++)
			{
				textlines[i] = new TextLine
				{
					Text = lines[i].Replace("\r\n", "").Replace("\n", ""),
					Bounds = new LineRectangled(0.0, (double)i * fontHeight, Bounds.InnerWidth, fontHeight)
				};
			}
			textUtil.DrawMultilineTextAt(ctx, Font, textlines, Bounds.absPaddingX + leftPadding, Bounds.absPaddingY, width);
		}
		else
		{
			topPadding = Math.Max(0.0, Bounds.OuterHeight - bottomSpacing - ctx.FontExtents.Height) / 2.0;
			textUtil.DrawTextLine(ctx, Font, displayedText, Bounds.absPaddingX + leftPadding, Bounds.absPaddingY + topPadding);
		}
		generateTexture(surface, ref textTexture);
		ctx.Dispose();
		surface.Dispose();
		if (caretTexture.TextureId == 0)
		{
			caretHeight = fontHeight;
			surface = new ImageSurface(Format.Argb32, 3, (int)fontHeight);
			ctx = genContext(surface);
			Font.SetupContext(ctx);
			ctx.SetSourceRGBA(caretColor[0], caretColor[1], caretColor[2], caretColor[3]);
			ctx.LineWidth = 1.0;
			ctx.NewPath();
			ctx.MoveTo(2.0, 0.0);
			ctx.LineTo(2.0, fontHeight);
			ctx.ClosePath();
			ctx.Stroke();
			generateTexture(surface, ref caretTexture.TextureId);
			ctx.Dispose();
			surface.Dispose();
		}
	}

	private void DeleteSelectedText(int caretPos, int caretPosOffset)
	{
		TextSelection textSelection = GetSelection(caretPos);
		SetValue(GetText().Remove(textSelection.Start, textSelection.End - textSelection.Start), setCaretPosToEnd: false);
		selectedTextStart = null;
		if (caretPos == textSelection.End)
		{
			CaretPosWithoutLineBreaks = textSelection.Start + caretPosOffset;
		}
	}

	private static int GetCharRank(char c)
	{
		if (IsWordChar(c))
		{
			return 3;
		}
		if (!char.IsWhiteSpace(c))
		{
			return 2;
		}
		return 1;
	}

	private void SelectWordAtCursor()
	{
		string line = lines[CaretPosLine];
		int caretPosInLine = CaretPosInLine;
		int targetRank = ((caretPosInLine > 0) ? GetCharRank(line[caretPosInLine - 1]) : 0);
		if (caretPosInLine < line.Length)
		{
			targetRank = Math.Max(targetRank, GetCharRank(line[caretPosInLine]));
		}
		int start = caretPosInLine;
		while (start > 0 && GetCharRank(line[start - 1]) == targetRank)
		{
			start--;
		}
		int end;
		for (end = caretPosInLine; end < line.Length && GetCharRank(line[end]) == targetRank; end++)
		{
		}
		if (start != caretPosInLine || end != caretPosInLine)
		{
			selectedTextStart = CaretPosWithoutLineBreaks - (caretPosInLine - start);
			SetCaretPos(end, CaretPosLine);
		}
	}

	private TextSelection GetSelection(int caretPos)
	{
		return new TextSelection(Math.Min(selectedTextStart.Value, caretPos), Math.Max(selectedTextStart.Value, caretPos));
	}

	public override void OnMouseDownOnElement(ICoreClientAPI api, MouseEvent args)
	{
		base.OnMouseDownOnElement(api, args);
		if (args.Button != EnumMouseButton.Left)
		{
			return;
		}
		bool num = ((Func<bool>)(() => api.Input.KeyboardKeyStateRaw[1] || api.Input.KeyboardKeyStateRaw[2]))();
		if (num && !selectedTextStart.HasValue)
		{
			selectedTextStart = CaretPosWithoutLineBreaks;
		}
		SetCaretPos((double)args.X - Bounds.absX, (double)args.Y - Bounds.absY);
		if (!num)
		{
			long now = api.ElapsedMilliseconds;
			int caretPos = CaretPosWithoutLineBreaks;
			bool num2 = lastClickCursor == caretPos && now - lastClickTime < 400;
			lastClickTime = now;
			lastClickCursor = caretPos;
			if (num2)
			{
				SelectWordAtCursor();
				return;
			}
			selectedTextStart = caretPos;
		}
		mouseDown = true;
	}

	public override void OnMouseMove(ICoreClientAPI api, MouseEvent args)
	{
		base.OnMouseMove(api, args);
		if (mouseDown)
		{
			SetCaretPos((double)args.X - Bounds.absX, (double)args.Y - Bounds.absY);
		}
	}

	public override void OnMouseUp(ICoreClientAPI api, MouseEvent args)
	{
		base.OnMouseUp(api, args);
		if (args.Button == EnumMouseButton.Left)
		{
			mouseDown = false;
			if (selectedTextStart == CaretPosWithoutLineBreaks)
			{
				selectedTextStart = null;
			}
		}
	}

	public override void OnKeyDown(ICoreClientAPI api, KeyEvent args)
	{
		if (base.HasFocus)
		{
			handlingOnKeyEvent = true;
			OnKeyDownInternal(api, args);
			handlingOnKeyEvent = false;
		}
	}

	private void OnKeyDownInternal(ICoreClientAPI api, KeyEvent args)
	{
		if (args.AltPressed)
		{
			args.Handled = true;
			return;
		}
		if ((args.CtrlPressed || args.CommandPressed) && OnControlAction(args))
		{
			api.Gui.PlaySound("tick");
			args.Handled = true;
			return;
		}
		bool handled = multilineMode || args.KeyCode != 52;
		bool flag = enabled;
		bool flag2;
		if (flag)
		{
			int keyCode = args.KeyCode;
			flag2 = ((keyCode == 53 || keyCode == 55) ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			if (!args.CtrlPressed || !OnDeleteWord((args.KeyCode != 53) ? 1 : (-1)))
			{
				if (!selectedTextStart.HasValue)
				{
					if (args.KeyCode == 53)
					{
						OnKeyBackSpace();
					}
					else
					{
						OnKeyDelete();
					}
				}
				else
				{
					DeleteSelectedText(CaretPosWithoutLineBreaks, 0);
					api.Gui.PlaySound("tick");
				}
			}
			args.Handled = true;
			return;
		}
		flag = args.ShiftPressed != selectedTextStart.HasValue;
		if (flag)
		{
			int keyCode = args.KeyCode;
			flag2 = (((uint)(keyCode - 45) <= 3u || (uint)(keyCode - 58) <= 1u) ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			if (!args.CtrlPressed && selectedTextStart.HasValue)
			{
				int caretPos = CaretPosWithoutLineBreaks;
				flag = selectedTextStart < caretPos;
				int keyCode;
				if (flag)
				{
					keyCode = args.KeyCode;
					bool flag3 = ((keyCode == 45 || keyCode == 47 || keyCode == 58) ? true : false);
					flag = flag3;
				}
				flag2 = flag;
				if (!flag2)
				{
					bool flag3 = selectedTextStart > caretPos;
					if (flag3)
					{
						keyCode = args.KeyCode;
						bool flag4 = ((keyCode == 46 || keyCode == 48 || keyCode == 59) ? true : false);
						flag3 = flag4;
					}
					flag2 = flag3;
				}
				if (flag2)
				{
					CaretPosWithoutLineBreaks = selectedTextStart.Value;
				}
				keyCode = args.KeyCode;
				if ((uint)(keyCode - 47) <= 1u)
				{
					selectedTextStart = null;
					args.Handled = true;
					api.Gui.PlaySound("tick");
					return;
				}
			}
			selectedTextStart = (args.ShiftPressed ? new int?(CaretPosWithoutLineBreaks) : ((int?)null));
		}
		if (args.KeyCode == 59)
		{
			if (args.CtrlPressed)
			{
				SetCaretPos(lines[lines.Count - 1].TrimEnd(new char[2] { '\r', '\n' }).Length, lines.Count - 1);
			}
			else
			{
				SetCaretPos(lines[CaretPosLine].TrimEnd(new char[2] { '\r', '\n' }).Length, CaretPosLine);
			}
			api.Gui.PlaySound("tick");
		}
		if (args.KeyCode == 58)
		{
			if (args.CtrlPressed)
			{
				SetCaretPos(0);
			}
			else
			{
				SetCaretPos(0, CaretPosLine);
			}
			api.Gui.PlaySound("tick");
		}
		if (args.KeyCode == 47)
		{
			MoveCursor(-1, args.CtrlPressed);
		}
		if (args.KeyCode == 48)
		{
			MoveCursor(1, args.CtrlPressed);
		}
		if (args.KeyCode == 46 && CaretPosLine < lines.Count - 1)
		{
			SetCaretPos(CaretPosInLine, CaretPosLine + 1);
			api.Gui.PlaySound("tick");
		}
		if (args.KeyCode == 45 && CaretPosLine > 0)
		{
			SetCaretPos(CaretPosInLine, CaretPosLine - 1);
			api.Gui.PlaySound("tick");
		}
		if (!mouseDown && selectedTextStart == CaretPosWithoutLineBreaks)
		{
			selectedTextStart = null;
		}
		flag2 = enabled;
		if (flag2)
		{
			int keyCode = args.KeyCode;
			flag = ((keyCode == 49 || keyCode == 82) ? true : false);
			flag2 = flag;
		}
		if (flag2)
		{
			if (multilineMode)
			{
				OnKeyEnter();
			}
			else
			{
				handled = false;
			}
		}
		if (args.KeyCode == 50)
		{
			handled = false;
		}
		args.Handled = handled;
	}

	public override string GetText()
	{
		return string.Join("", lines);
	}

	private bool OnControlAction(KeyEvent args)
	{
		string keyString = GlKeyNames.GetPrintableChar(args.KeyCode);
		switch (keyString)
		{
		case "a":
		{
			selectedTextStart = 0;
			List<string> list = lines;
			SetCaretPos(list[list.Count - 1].Length, lines.Count - 1);
			return true;
		}
		case "c":
			return OnCopyCut(CopyCutMode.Copy);
		case "x":
			if (enabled)
			{
				return OnCopyCut(CopyCutMode.Cut);
			}
			break;
		}
		if (keyString == "v" && enabled)
		{
			return OnPaste();
		}
		return false;
	}

	private bool OnCopyCut(CopyCutMode mode)
	{
		if (!selectedTextStart.HasValue)
		{
			return false;
		}
		string obj = GetText();
		int caretPos = CaretPosWithoutLineBreaks;
		TextSelection textSelection = GetSelection(caretPos);
		int start = textSelection.Start;
		string subtext = obj.Substring(start, textSelection.End - start);
		if (subtext.Length != 0)
		{
			api.Forms.SetClipboardText(subtext);
		}
		if (mode == CopyCutMode.Cut)
		{
			DeleteSelectedText(caretPos, 0);
		}
		return true;
	}

	private bool OnPaste()
	{
		if (selectedTextStart.HasValue)
		{
			DeleteSelectedText(CaretPosWithoutLineBreaks, 0);
		}
		string insert = api.Forms.GetClipboardText();
		insert = insert.Replace("\ufeff", "");
		if (!multilineMode)
		{
			insert = insert.Replace("\r", "").Replace("\n", "");
		}
		string fulltext = string.Join("", lines);
		int caretPos = CaretPosWithoutLineBreaks;
		string obj = fulltext.Substring(0, caretPos);
		string obj2 = insert;
		string text = fulltext;
		int num = caretPos;
		SetValue(obj + obj2 + text.Substring(num, text.Length - num), setCaretPosToEnd: false);
		CaretPosWithoutLineBreaks = caretPos + insert.Length;
		return true;
	}

	private bool OnDeleteWord(int direction)
	{
		if (selectedTextStart.HasValue)
		{
			return false;
		}
		selectedTextStart = CaretPosWithoutLineBreaks;
		MoveCursor(direction, wholeWord: true, wholeWordWithWhitespace: true);
		DeleteSelectedText(CaretPosWithoutLineBreaks, 0);
		return true;
	}

	private void OnKeyEnter()
	{
		if (selectedTextStart.HasValue)
		{
			DeleteSelectedText(CaretPosWithoutLineBreaks, 0);
		}
		if (lines.Count < maxlines)
		{
			string leftText = linesStaging[CaretPosLine].Substring(0, CaretPosInLine);
			string rightText = linesStaging[CaretPosLine].Substring(CaretPosInLine);
			linesStaging[CaretPosLine] = leftText + "\n";
			linesStaging.Insert(CaretPosLine + 1, rightText);
			OnTryTextChangeDelegate onTryTextChangeText = OnTryTextChangeText;
			if (onTryTextChangeText == null || onTryTextChangeText(linesStaging))
			{
				lines = new List<string>(linesStaging);
				TextChanged();
				SetCaretPos(0, CaretPosLine + 1);
				api.Gui.PlaySound("tick");
			}
		}
	}

	private void OnKeyDelete()
	{
		string alltext = GetText();
		int caret = CaretPosWithoutLineBreaks;
		if (alltext.Length != caret)
		{
			alltext = alltext.Substring(0, caret) + alltext.Substring(caret + 1, alltext.Length - caret - 1);
			LoadValue(Lineize(alltext));
			api.Gui.PlaySound("tick");
		}
	}

	private void OnKeyBackSpace()
	{
		int caret = CaretPosWithoutLineBreaks;
		if (caret != 0)
		{
			string alltext = GetText();
			alltext = alltext.Substring(0, caret - 1) + alltext.Substring(caret, alltext.Length - caret);
			int cpos = CaretPosWithoutLineBreaks;
			LoadValue(Lineize(alltext));
			if (cpos > 0)
			{
				CaretPosWithoutLineBreaks = cpos - 1;
			}
			api.Gui.PlaySound("tick");
		}
	}

	public override void OnKeyPress(ICoreClientAPI api, KeyEvent args)
	{
		if (!base.HasFocus || !enabled || isLengthCapped())
		{
			return;
		}
		if (selectedTextStart.HasValue)
		{
			DeleteSelectedText(CaretPosWithoutLineBreaks, 0);
		}
		string newline = lines[CaretPosLine].Substring(0, CaretPosInLine) + args.KeyChar + lines[CaretPosLine].Substring(CaretPosInLine, lines[CaretPosLine].Length - CaretPosInLine);
		double width = Bounds.InnerWidth - 2.0 * Bounds.absPaddingX - rightSpacing;
		linesStaging[CaretPosLine] = newline;
		if (multilineMode && Font.GetTextExtents(newline.TrimEnd(new char[2] { '\r', '\n' })).Width >= width)
		{
			StringBuilder newLines = new StringBuilder();
			for (int i = 0; i < lines.Count; i++)
			{
				newLines.Append((i == CaretPosLine) ? newline : lines[i]);
			}
			linesStaging = Lineize(newLines.ToString());
			if (lines.Count >= maxlines && linesStaging.Count >= maxlines)
			{
				return;
			}
		}
		handlingOnKeyEvent = true;
		LoadValue(linesStaging);
		CaretPosWithoutLineBreaks++;
		handlingOnKeyEvent = false;
		args.Handled = true;
		api.Gui.PlaySound("tick");
		OnKeyPressed?.Invoke();
	}

	private bool isLengthCapped()
	{
		if (maxlength < 0)
		{
			return false;
		}
		int len = 0;
		foreach (string line in lines)
		{
			len += line.Length;
		}
		return len >= maxlength;
	}

	public override void RenderInteractiveElements(float deltaTime)
	{
		if (base.HasFocus)
		{
			if (api.ElapsedMilliseconds - caretBlinkMilliseconds > 900)
			{
				caretBlinkMilliseconds = api.ElapsedMilliseconds;
				caretDisplayed = !caretDisplayed;
			}
			if (caretDisplayed && caretX - renderLeftOffset < Bounds.InnerWidth)
			{
				api.Render.Render2DTexturePremultipliedAlpha(caretTexture.TextureId, Bounds.renderX + caretX + GuiElement.scaled(1.5) - renderLeftOffset, Bounds.renderY + caretY + topPadding, 2.0, caretHeight);
			}
		}
	}

	protected void RenderTextSelection()
	{
		if (!selectedTextStart.HasValue)
		{
			return;
		}
		TextSelection textSelection = GetSelection(CaretPosWithoutLineBreaks);
		SelectedTextPos start = GetPosition(textSelection.Start);
		SelectedTextPos end = GetPosition(textSelection.End);
		if (start.Y == end.Y)
		{
			RenderSelectionLine(start.X, end.X, start.Y);
			return;
		}
		RenderSelectionLine(start.X, -1, start.Y);
		for (int lineIndex = start.Y + 1; lineIndex < end.Y; lineIndex++)
		{
			RenderSelectionLine(0, -1, lineIndex);
		}
		RenderSelectionLine(0, end.X, end.Y);
	}

	private void RenderSelectionLine(int fromX, int toX, int lineIndex)
	{
		double num = Bounds.renderX + leftPadding;
		double renderY = Bounds.renderY + topPadding;
		double height = Font.GetFontExtents().Height;
		double x = num + ((fromX == 0) ? 0.0 : Font.GetTextExtents(lines[lineIndex].Substring(0, fromX)).XAdvance);
		double y = renderY + height * (double)lineIndex;
		double width = Font.GetTextExtents(lines[lineIndex].Substring(fromX, ((toX == -1) ? lines[lineIndex].Length : toX) - fromX)).XAdvance;
		api.Render.Render2DTexturePremultipliedAlpha(selectionTextureId, x - renderLeftOffset, y, width, height);
	}

	private SelectedTextPos GetPosition(int positionWithoutLineBreaks)
	{
		int linePos = 0;
		foreach (string line in lines)
		{
			if (positionWithoutLineBreaks > line.Length)
			{
				linePos++;
				positionWithoutLineBreaks -= line.Length;
				continue;
			}
			break;
		}
		return new SelectedTextPos(positionWithoutLineBreaks, linePos);
	}

	public override void Dispose()
	{
		base.Dispose();
		caretTexture.Dispose();
		textTexture.Dispose();
		if (selectionTextureId != 0)
		{
			api.Gui.DeleteTexture(selectionTextureId);
			selectionTextureId = 0;
		}
	}

	/// <summary>
	/// Moves the cursor forward and backward by an amount.
	/// </summary>
	/// <param name="dir">The direction to move the cursor.</param>
	/// <param name="wholeWord">Whether or not we skip entire words moving it.</param>
	/// <param name="wholeWordWithWhitespace">Force the cursor to skip whitespace after a word.</param>
	public void MoveCursor(int dir, bool wholeWord = false, bool wholeWordWithWhitespace = false)
	{
		bool moved = ((CaretPosInLine > 0 || CaretPosLine > 0) && dir < 0) || ((CaretPosInLine < lines[CaretPosLine].Length || CaretPosLine < lines.Count - 1) && dir > 0);
		if (wholeWord)
		{
			dir = ((dir >= 0) ? 1 : (-1));
			string text = GetText();
			int stop = ((dir < 0) ? (-1) : text.Length);
			int offset = ((dir < 0) ? (-1) : 0);
			int caretPos = CaretPosWithoutLineBreaks + offset;
			bool startsWithWhitespace = caretPos != stop && char.IsWhiteSpace(text[caretPos]);
			for (; caretPos != stop && char.IsWhiteSpace(text[caretPos]); caretPos += dir)
			{
			}
			if (caretPos != stop && !IsWordChar(text[caretPos]))
			{
				for (; caretPos != stop && !IsWordChar(text[caretPos]) && !char.IsWhiteSpace(text[caretPos]); caretPos += dir)
				{
				}
			}
			else
			{
				for (; caretPos != stop && IsWordChar(text[caretPos]); caretPos += dir)
				{
				}
			}
			if (!startsWithWhitespace && wholeWordWithWhitespace)
			{
				for (; caretPos != stop && char.IsWhiteSpace(text[caretPos]); caretPos += dir)
				{
				}
			}
			CaretPosWithoutLineBreaks = caretPos - offset;
		}
		else
		{
			CaretPosWithoutLineBreaks += dir;
		}
		if (moved)
		{
			api.Gui.PlaySound("tick");
		}
	}

	/// <summary>
	/// Sets the number of lines in the Text Area.
	/// </summary>
	/// <param name="maxlines">The maximum number of lines.</param>
	public void SetMaxLines(int maxlines)
	{
		this.maxlines = maxlines;
	}

	/// <summary>
	/// Limits the number of characters a user may enter (default: no limit). Use -1 to set no limit
	/// </summary>
	/// <param name="maxlength"></param>
	public void SetMaxLength(int maxlength)
	{
		this.maxlength = maxlength;
	}

	public void SetMaxHeight(int maxheight)
	{
		maxlines = (int)Math.Floor((double)maxheight / Font.GetFontExtents().Height);
	}

	private static int GenerateSelectionTexture(ICoreClientAPI api)
	{
		using ImageSurface surface = new ImageSurface(Format.Argb32, 32, 32);
		using Context context = new Context(surface);
		context.SetSourceRGBA(0.0, 0.75, 1.0, 0.5);
		context.Paint();
		return api.Gui.LoadCairoTexture(surface, linearMag: true);
	}

	private static bool IsWordChar(char c)
	{
		if (c != '_')
		{
			return char.IsLetterOrDigit(c);
		}
		return true;
	}
}
