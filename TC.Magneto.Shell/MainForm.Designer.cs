namespace TC.Magneto.Shell
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.MenuStrip MenuStrip;
			System.Windows.Forms.ToolStripMenuItem MenuItemFile;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemNew;
			TC.WinForms.Commands.ApplicationCommand NewDocumentCommand;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemOpen;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuFile1;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemSave;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemSaveAs;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuFile2;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemRun;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemStop;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuFile3;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemClose;
			System.Windows.Forms.ToolStripMenuItem MenuItemEdit;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemUndo;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemRedo;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuEdit1;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemCut;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemCopy;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemPaste;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemDelete;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuEdit2;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemSelectAll;
			System.Windows.Forms.StatusStrip StatusStrip;
			System.Windows.Forms.ToolStrip ToolStrip;
			TC.WinForms.Controls.TToolStripCommandButton ButtonNew;
			TC.WinForms.Controls.TToolStripCommandButton ButtonOpen;
			TC.WinForms.Controls.TToolStripCommandButton ButtonSave;
			System.Windows.Forms.ToolStripSeparator SeparatorToolStrip1;
			TC.WinForms.Controls.TToolStripCommandButton ButtonCut;
			TC.WinForms.Controls.TToolStripCommandButton ButtonCopy;
			TC.WinForms.Controls.TToolStripCommandButton ButtonPaste;
			System.Windows.Forms.ToolStripSeparator SeparatorToolStrip2;
			TC.WinForms.Controls.TToolStripCommandButton ButtonUndo;
			TC.WinForms.Controls.TToolStripCommandButton ButtonRedo;
			System.Windows.Forms.ToolStripSeparator SeparatorToolStrip3;
			TC.WinForms.Controls.TToolStripCommandButton ButtonRun;
			TC.WinForms.Controls.TToolStripCommandButton ButtonStop;
			TC.WinForms.Controls.TToolStripCommandButton ButtonInfo;
			TC.WinForms.Commands.ApplicationCommand LanguageGuideCommand;
			System.Windows.Forms.ToolStripMenuItem MenuItemHelp;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemLanguageGuide;
			TC.WinForms.Controls.TToolStripCommandMenuItem MenuItemAbout;
			System.Windows.Forms.ToolStripSeparator SeparatorMenuHelp1;
			this.RunCommand = new TC.WinForms.Commands.ApplicationCommand();
			this.StopCommand = new TC.WinForms.Commands.ApplicationCommand();
			this.CodeEditor = new TC.WinForms.Controls.TCodeEditor();
			this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.LabelCurrentLine = new System.Windows.Forms.ToolStripStatusLabel();
			this.LabelCurrentColumn = new System.Windows.Forms.ToolStripStatusLabel();
			this.LabelOverwriteMode = new System.Windows.Forms.ToolStripStatusLabel();
			MenuStrip = new System.Windows.Forms.MenuStrip();
			MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
			MenuItemNew = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			NewDocumentCommand = new TC.WinForms.Commands.ApplicationCommand();
			MenuItemOpen = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuFile1 = new System.Windows.Forms.ToolStripSeparator();
			MenuItemSave = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemSaveAs = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuFile2 = new System.Windows.Forms.ToolStripSeparator();
			MenuItemRun = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemStop = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuFile3 = new System.Windows.Forms.ToolStripSeparator();
			MenuItemClose = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
			MenuItemUndo = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemRedo = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuEdit1 = new System.Windows.Forms.ToolStripSeparator();
			MenuItemCut = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemCopy = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemPaste = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemDelete = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuEdit2 = new System.Windows.Forms.ToolStripSeparator();
			MenuItemSelectAll = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			StatusStrip = new System.Windows.Forms.StatusStrip();
			ToolStrip = new System.Windows.Forms.ToolStrip();
			ButtonNew = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonOpen = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonSave = new TC.WinForms.Controls.TToolStripCommandButton();
			SeparatorToolStrip1 = new System.Windows.Forms.ToolStripSeparator();
			ButtonCut = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonCopy = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonPaste = new TC.WinForms.Controls.TToolStripCommandButton();
			SeparatorToolStrip2 = new System.Windows.Forms.ToolStripSeparator();
			ButtonUndo = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonRedo = new TC.WinForms.Controls.TToolStripCommandButton();
			SeparatorToolStrip3 = new System.Windows.Forms.ToolStripSeparator();
			ButtonRun = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonStop = new TC.WinForms.Controls.TToolStripCommandButton();
			ButtonInfo = new TC.WinForms.Controls.TToolStripCommandButton();
			LanguageGuideCommand = new TC.WinForms.Commands.ApplicationCommand();
			MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
			MenuItemLanguageGuide = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			MenuItemAbout = new TC.WinForms.Controls.TToolStripCommandMenuItem();
			SeparatorMenuHelp1 = new System.Windows.Forms.ToolStripSeparator();
			MenuStrip.SuspendLayout();
			StatusStrip.SuspendLayout();
			ToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            MenuItemFile,
            MenuItemEdit,
            MenuItemHelp});
			MenuStrip.Location = new System.Drawing.Point(0, 0);
			MenuStrip.Name = "MenuStrip";
			MenuStrip.Padding = new System.Windows.Forms.Padding(0);
			MenuStrip.Size = new System.Drawing.Size(334, 24);
			MenuStrip.TabIndex = 0;
			// 
			// MenuItemFile
			// 
			MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            MenuItemNew,
            MenuItemOpen,
            SeparatorMenuFile1,
            MenuItemSave,
            MenuItemSaveAs,
            SeparatorMenuFile2,
            MenuItemRun,
            MenuItemStop,
            SeparatorMenuFile3,
            MenuItemClose});
			MenuItemFile.Name = "MenuItemFile";
			MenuItemFile.Size = new System.Drawing.Size(37, 24);
			MenuItemFile.Text = "&File";
			// 
			// MenuItemNew
			// 
			MenuItemNew.Command = NewDocumentCommand;
			MenuItemNew.Image = global::TC.Magneto.Shell.Properties.Resources.NewDocument;
			MenuItemNew.Name = "MenuItemNew";
			MenuItemNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			MenuItemNew.Size = new System.Drawing.Size(195, 22);
			MenuItemNew.Text = "&New";
			// 
			// NewDocumentCommand
			// 
			NewDocumentCommand.Executed += new System.EventHandler(this.NewDocument);
			// 
			// MenuItemOpen
			// 
			MenuItemOpen.Command = this.LoadDocumentCommand;
			MenuItemOpen.Image = global::TC.Magneto.Shell.Properties.Resources.Open;
			MenuItemOpen.Name = "MenuItemOpen";
			MenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			MenuItemOpen.Size = new System.Drawing.Size(195, 22);
			MenuItemOpen.Text = "&Open…";
			// 
			// SeparatorMenuFile1
			// 
			SeparatorMenuFile1.Name = "SeparatorMenuFile1";
			SeparatorMenuFile1.Size = new System.Drawing.Size(192, 6);
			// 
			// MenuItemSave
			// 
			MenuItemSave.Command = this.SaveDocumentCommand;
			MenuItemSave.Image = global::TC.Magneto.Shell.Properties.Resources.Save;
			MenuItemSave.Name = "MenuItemSave";
			MenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			MenuItemSave.Size = new System.Drawing.Size(195, 22);
			MenuItemSave.Text = "&Save";
			// 
			// MenuItemSaveAs
			// 
			MenuItemSaveAs.Command = this.SaveDocumentAsCommand;
			MenuItemSaveAs.Name = "MenuItemSaveAs";
			MenuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.S)));
			MenuItemSaveAs.Size = new System.Drawing.Size(195, 22);
			MenuItemSaveAs.Text = "Save &As…";
			// 
			// SeparatorMenuFile2
			// 
			SeparatorMenuFile2.Name = "SeparatorMenuFile2";
			SeparatorMenuFile2.Size = new System.Drawing.Size(192, 6);
			// 
			// MenuItemRun
			// 
			MenuItemRun.Command = this.RunCommand;
			MenuItemRun.Image = global::TC.Magneto.Shell.Properties.Resources.Play;
			MenuItemRun.Name = "MenuItemRun";
			MenuItemRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
			MenuItemRun.Size = new System.Drawing.Size(195, 22);
			MenuItemRun.Text = "&Run";
			// 
			// RunCommand
			// 
			this.RunCommand.Executed += new System.EventHandler(this.Run);
			// 
			// MenuItemStop
			// 
			MenuItemStop.Command = this.StopCommand;
			MenuItemStop.Image = global::TC.Magneto.Shell.Properties.Resources.Stop;
			MenuItemStop.Name = "MenuItemStop";
			MenuItemStop.Size = new System.Drawing.Size(195, 22);
			MenuItemStop.Text = "Stop";
			// 
			// StopCommand
			// 
			this.StopCommand.CanExecute = false;
			this.StopCommand.Executed += new System.EventHandler(this.Stop);
			// 
			// SeparatorMenuFile3
			// 
			SeparatorMenuFile3.Name = "SeparatorMenuFile3";
			SeparatorMenuFile3.Size = new System.Drawing.Size(192, 6);
			// 
			// MenuItemClose
			// 
			MenuItemClose.Command = this.CloseCommand;
			MenuItemClose.Name = "MenuItemClose";
			MenuItemClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			MenuItemClose.Size = new System.Drawing.Size(195, 22);
			MenuItemClose.Text = "&Close";
			// 
			// MenuItemEdit
			// 
			MenuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            MenuItemUndo,
            MenuItemRedo,
            SeparatorMenuEdit1,
            MenuItemCut,
            MenuItemCopy,
            MenuItemPaste,
            MenuItemDelete,
            SeparatorMenuEdit2,
            MenuItemSelectAll});
			MenuItemEdit.Name = "MenuItemEdit";
			MenuItemEdit.Size = new System.Drawing.Size(39, 24);
			MenuItemEdit.Text = "&Edit";
			// 
			// MenuItemUndo
			// 
			MenuItemUndo.Command = this.CodeEditor.UndoCommand;
			MenuItemUndo.Image = global::TC.Magneto.Shell.Properties.Resources.Undo;
			MenuItemUndo.Name = "MenuItemUndo";
			MenuItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			MenuItemUndo.Size = new System.Drawing.Size(164, 22);
			MenuItemUndo.Text = "&Undo";
			// 
			// CodeEditor
			// 
			this.CodeEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CodeEditor.FileDialogFilter = "Magneto Files (*.magneto)|*.magneto|All Files (*.*)|*.*";
			this.CodeEditor.Location = new System.Drawing.Point(0, 49);
			this.CodeEditor.Name = "CodeEditor";
			this.CodeEditor.Size = new System.Drawing.Size(334, 193);
			this.CodeEditor.TabIndex = 2;
			this.CodeEditor.Text = "";
			this.CodeEditor.CurrentLineNumberChanged += new System.EventHandler(this.SetLabelCurrentLine);
			this.CodeEditor.OverwriteModeChanged += new System.EventHandler(this.SetLabelOverwriteMode);
			this.CodeEditor.CurrentColumnNumberChanged += new System.EventHandler(this.SetLabelCurrentColumn);
			// 
			// MenuItemRedo
			// 
			MenuItemRedo.Command = this.CodeEditor.RedoCommand;
			MenuItemRedo.Image = global::TC.Magneto.Shell.Properties.Resources.Redo;
			MenuItemRedo.Name = "MenuItemRedo";
			MenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			MenuItemRedo.Size = new System.Drawing.Size(164, 22);
			MenuItemRedo.Text = "&Redo";
			// 
			// SeparatorMenuEdit1
			// 
			SeparatorMenuEdit1.Name = "SeparatorMenuEdit1";
			SeparatorMenuEdit1.Size = new System.Drawing.Size(161, 6);
			// 
			// MenuItemCut
			// 
			MenuItemCut.Command = this.CodeEditor.CutCommand;
			MenuItemCut.Image = global::TC.Magneto.Shell.Properties.Resources.Cut;
			MenuItemCut.Name = "MenuItemCut";
			MenuItemCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			MenuItemCut.Size = new System.Drawing.Size(164, 22);
			MenuItemCut.Text = "Cu&t";
			// 
			// MenuItemCopy
			// 
			MenuItemCopy.Command = this.CodeEditor.CopyCommand;
			MenuItemCopy.Image = global::TC.Magneto.Shell.Properties.Resources.Copy;
			MenuItemCopy.Name = "MenuItemCopy";
			MenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			MenuItemCopy.Size = new System.Drawing.Size(164, 22);
			MenuItemCopy.Text = "&Copy";
			// 
			// MenuItemPaste
			// 
			MenuItemPaste.Command = this.CodeEditor.PasteCommand;
			MenuItemPaste.Image = global::TC.Magneto.Shell.Properties.Resources.Paste;
			MenuItemPaste.Name = "MenuItemPaste";
			MenuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			MenuItemPaste.Size = new System.Drawing.Size(164, 22);
			MenuItemPaste.Text = "&Paste";
			// 
			// MenuItemDelete
			// 
			MenuItemDelete.Command = this.CodeEditor.DeleteCommand;
			MenuItemDelete.Image = global::TC.Magneto.Shell.Properties.Resources.Delete;
			MenuItemDelete.Name = "MenuItemDelete";
			MenuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			MenuItemDelete.Size = new System.Drawing.Size(164, 22);
			MenuItemDelete.Text = "&Delete";
			// 
			// SeparatorMenuEdit2
			// 
			SeparatorMenuEdit2.Name = "SeparatorMenuEdit2";
			SeparatorMenuEdit2.Size = new System.Drawing.Size(161, 6);
			// 
			// MenuItemSelectAll
			// 
			MenuItemSelectAll.Command = this.CodeEditor.SelectAllCommand;
			MenuItemSelectAll.Name = "MenuItemSelectAll";
			MenuItemSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			MenuItemSelectAll.Size = new System.Drawing.Size(164, 22);
			MenuItemSelectAll.Text = "Select &All";
			// 
			// StatusStrip
			// 
			StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelStatus,
            this.LabelCurrentLine,
            this.LabelCurrentColumn,
            this.LabelOverwriteMode});
			StatusStrip.Location = new System.Drawing.Point(0, 242);
			StatusStrip.Name = "StatusStrip";
			StatusStrip.Size = new System.Drawing.Size(334, 22);
			StatusStrip.TabIndex = 1;
			// 
			// LabelStatus
			// 
			this.LabelStatus.Name = "LabelStatus";
			this.LabelStatus.Size = new System.Drawing.Size(231, 17);
			this.LabelStatus.Spring = true;
			this.LabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelCurrentLine
			// 
			this.LabelCurrentLine.Name = "LabelCurrentLine";
			this.LabelCurrentLine.Size = new System.Drawing.Size(29, 17);
			this.LabelCurrentLine.Text = "Ln 1";
			// 
			// LabelCurrentColumn
			// 
			this.LabelCurrentColumn.Name = "LabelCurrentColumn";
			this.LabelCurrentColumn.Size = new System.Drawing.Size(34, 17);
			this.LabelCurrentColumn.Text = "Col 1";
			// 
			// LabelOverwriteMode
			// 
			this.LabelOverwriteMode.Name = "LabelOverwriteMode";
			this.LabelOverwriteMode.Size = new System.Drawing.Size(25, 17);
			this.LabelOverwriteMode.Text = "INS";
			// 
			// ToolStrip
			// 
			ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            ButtonNew,
            ButtonOpen,
            ButtonSave,
            SeparatorToolStrip1,
            ButtonCut,
            ButtonCopy,
            ButtonPaste,
            SeparatorToolStrip2,
            ButtonUndo,
            ButtonRedo,
            SeparatorToolStrip3,
            ButtonRun,
            ButtonStop,
            ButtonInfo});
			ToolStrip.Location = new System.Drawing.Point(0, 24);
			ToolStrip.Name = "ToolStrip";
			ToolStrip.Size = new System.Drawing.Size(334, 25);
			ToolStrip.TabIndex = 3;
			// 
			// ButtonNew
			// 
			ButtonNew.Command = NewDocumentCommand;
			ButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonNew.Image = global::TC.Magneto.Shell.Properties.Resources.NewDocument;
			ButtonNew.Name = "ButtonNew";
			ButtonNew.Size = new System.Drawing.Size(23, 22);
			ButtonNew.ToolTipText = "New (Ctrl+N)";
			// 
			// ButtonOpen
			// 
			ButtonOpen.Command = this.LoadDocumentCommand;
			ButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonOpen.Image = global::TC.Magneto.Shell.Properties.Resources.Open;
			ButtonOpen.Name = "ButtonOpen";
			ButtonOpen.Size = new System.Drawing.Size(23, 22);
			ButtonOpen.ToolTipText = "Open (Ctrl+O)";
			// 
			// ButtonSave
			// 
			ButtonSave.Command = this.SaveDocumentCommand;
			ButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonSave.Image = global::TC.Magneto.Shell.Properties.Resources.Save;
			ButtonSave.Name = "ButtonSave";
			ButtonSave.Size = new System.Drawing.Size(23, 22);
			ButtonSave.ToolTipText = "Save (Ctrl+S)";
			// 
			// SeparatorToolStrip1
			// 
			SeparatorToolStrip1.Name = "SeparatorToolStrip1";
			SeparatorToolStrip1.Size = new System.Drawing.Size(6, 25);
			// 
			// ButtonCut
			// 
			ButtonCut.Command = this.CodeEditor.CutCommand;
			ButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonCut.Image = global::TC.Magneto.Shell.Properties.Resources.Cut;
			ButtonCut.Name = "ButtonCut";
			ButtonCut.Size = new System.Drawing.Size(23, 22);
			ButtonCut.ToolTipText = "Cut (Ctrl+X)";
			// 
			// ButtonCopy
			// 
			ButtonCopy.Command = this.CodeEditor.CopyCommand;
			ButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonCopy.Image = global::TC.Magneto.Shell.Properties.Resources.Copy;
			ButtonCopy.Name = "ButtonCopy";
			ButtonCopy.Size = new System.Drawing.Size(23, 22);
			ButtonCopy.ToolTipText = "Copy (Ctrl+C)";
			// 
			// ButtonPaste
			// 
			ButtonPaste.Command = this.CodeEditor.PasteCommand;
			ButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonPaste.Image = global::TC.Magneto.Shell.Properties.Resources.Paste;
			ButtonPaste.Name = "ButtonPaste";
			ButtonPaste.Size = new System.Drawing.Size(23, 22);
			ButtonPaste.ToolTipText = "Paste (Ctrl+V)";
			// 
			// SeparatorToolStrip2
			// 
			SeparatorToolStrip2.Name = "SeparatorToolStrip2";
			SeparatorToolStrip2.Size = new System.Drawing.Size(6, 25);
			// 
			// ButtonUndo
			// 
			ButtonUndo.Command = this.CodeEditor.UndoCommand;
			ButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonUndo.Image = global::TC.Magneto.Shell.Properties.Resources.Undo;
			ButtonUndo.Name = "ButtonUndo";
			ButtonUndo.Size = new System.Drawing.Size(23, 22);
			ButtonUndo.ToolTipText = "Undo (Ctrl+Z)";
			// 
			// ButtonRedo
			// 
			ButtonRedo.Command = this.CodeEditor.RedoCommand;
			ButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonRedo.Image = global::TC.Magneto.Shell.Properties.Resources.Redo;
			ButtonRedo.Name = "ButtonRedo";
			ButtonRedo.Size = new System.Drawing.Size(23, 22);
			ButtonRedo.ToolTipText = "Redo (Ctrl+Y)";
			// 
			// SeparatorToolStrip3
			// 
			SeparatorToolStrip3.Name = "SeparatorToolStrip3";
			SeparatorToolStrip3.Size = new System.Drawing.Size(6, 25);
			// 
			// ButtonRun
			// 
			ButtonRun.Command = this.RunCommand;
			ButtonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonRun.Image = global::TC.Magneto.Shell.Properties.Resources.Play;
			ButtonRun.Name = "ButtonRun";
			ButtonRun.Size = new System.Drawing.Size(23, 22);
			ButtonRun.ToolTipText = "Run (F5)";
			// 
			// ButtonStop
			// 
			ButtonStop.Command = this.StopCommand;
			ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonStop.Image = global::TC.Magneto.Shell.Properties.Resources.Stop;
			ButtonStop.Name = "ButtonStop";
			ButtonStop.Size = new System.Drawing.Size(23, 22);
			ButtonStop.ToolTipText = "Stop";
			// 
			// ButtonInfo
			// 
			ButtonInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			ButtonInfo.Command = this.ApplicationAboutCommand;
			ButtonInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ButtonInfo.Image = global::TC.Magneto.Shell.Properties.Resources.Info;
			ButtonInfo.Name = "ButtonInfo";
			ButtonInfo.Size = new System.Drawing.Size(23, 22);
			ButtonInfo.Text = "About Magneto";
			// 
			// LanguageGuideCommand
			// 
			LanguageGuideCommand.Executed += new System.EventHandler(this.DisplayLanguageGuide);
			// 
			// MenuItemHelp
			// 
			MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            MenuItemLanguageGuide,
            SeparatorMenuHelp1,
            MenuItemAbout});
			MenuItemHelp.Name = "MenuItemHelp";
			MenuItemHelp.Size = new System.Drawing.Size(44, 24);
			MenuItemHelp.Text = "&Help";
			// 
			// MenuItemLanguageGuide
			// 
			MenuItemLanguageGuide.Command = LanguageGuideCommand;
			MenuItemLanguageGuide.Name = "MenuItemLanguageGuide";
			MenuItemLanguageGuide.Size = new System.Drawing.Size(160, 22);
			MenuItemLanguageGuide.Text = "&Language Guide";
			// 
			// MenuItemAbout
			// 
			MenuItemAbout.Command = this.ApplicationAboutCommand;
			MenuItemAbout.Name = "MenuItemAbout";
			MenuItemAbout.Size = new System.Drawing.Size(160, 22);
			MenuItemAbout.Text = "&About Magneto";
			// 
			// SeparatorMenuHelp1
			// 
			SeparatorMenuHelp1.Name = "SeparatorMenuHelp1";
			SeparatorMenuHelp1.Size = new System.Drawing.Size(157, 6);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 264);
			this.Commands.Add(NewDocumentCommand);
			this.Commands.Add(this.RunCommand);
			this.Commands.Add(this.StopCommand);
			this.Commands.Add(LanguageGuideCommand);
			this.Controls.Add(this.CodeEditor);
			this.Controls.Add(StatusStrip);
			this.Controls.Add(ToolStrip);
			this.Controls.Add(MenuStrip);
			this.DocumentContainer = this.CodeEditor;
			this.MainMenuStrip = MenuStrip;
			this.MinimumSize = new System.Drawing.Size(350, 200);
			this.Name = "MainForm";
			this.Text = "Magneto";
			MenuStrip.ResumeLayout(false);
			MenuStrip.PerformLayout();
			StatusStrip.ResumeLayout(false);
			StatusStrip.PerformLayout();
			ToolStrip.ResumeLayout(false);
			ToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripStatusLabel LabelStatus;
		private System.Windows.Forms.ToolStripStatusLabel LabelCurrentLine;
		private System.Windows.Forms.ToolStripStatusLabel LabelCurrentColumn;
		private System.Windows.Forms.ToolStripStatusLabel LabelOverwriteMode;
		private TC.WinForms.Controls.TCodeEditor CodeEditor;
		private TC.WinForms.Commands.ApplicationCommand RunCommand;
		private TC.WinForms.Commands.ApplicationCommand StopCommand;



	}
}