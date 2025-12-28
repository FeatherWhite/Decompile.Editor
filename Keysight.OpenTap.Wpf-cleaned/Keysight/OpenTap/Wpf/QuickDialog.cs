// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.QuickDialog
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class QuickDialog
{
  private QuickDialog()
  {
  }

  public string Title { get; set; }

  public string Question { get; set; }

  public string PositiveAnswer { get; set; } = "Yes";

  public string NegativeAnswer { get; set; }

  public string CancelContent { get; set; } = "Cancel";

  public bool DontAskAgainEnabled { get; set; }

  public bool DontAskAgain { get; private set; }

  public static void ShowMessage(string title, string message)
  {
    int num = (int) QuickDialog.Show(title, message, "Ok", CancelContent: (string) null);
  }

  public static QuickDialog.DialogOption Show(
    string title,
    string message,
    string PositiveAnswer = "Yes",
    string NegativeAnswer = null,
    string CancelContent = "Cancel")
  {
    return new QuickDialog()
    {
      Title = title,
      Question = message,
      PositiveAnswer = PositiveAnswer,
      NegativeAnswer = NegativeAnswer,
      CancelContent = CancelContent
    }.ShowYesNoDialog2();
  }

  public static QuickDialog.DialogOption Show(
    string title,
    string message,
    out bool DontAskAgain,
    string PositiveAnswer = "Yes",
    string NegativeAnswer = null,
    string CancelContent = "Cancel")
  {
    QuickDialog quickDialog = new QuickDialog()
    {
      Title = title,
      Question = message,
      PositiveAnswer = PositiveAnswer,
      NegativeAnswer = NegativeAnswer,
      CancelContent = CancelContent,
      DontAskAgainEnabled = true
    };
    int num = (int) quickDialog.ShowYesNoDialog2();
    DontAskAgain = quickDialog.DontAskAgain;
    return (QuickDialog.DialogOption) num;
  }

  public QuickDialog.DialogOption ShowYesNoDialog2()
  {
    if (this.PositiveAnswer == this.NegativeAnswer || this.PositiveAnswer == this.CancelContent || this.NegativeAnswer == this.CancelContent && this.NegativeAnswer != null)
      throw new Exception("Dialog options are not distinct!");
    Dictionary<string, QuickDialog.DialogOption> dictionary = new Dictionary<string, QuickDialog.DialogOption>();
    if (!string.IsNullOrWhiteSpace(this.PositiveAnswer))
      dictionary[this.PositiveAnswer] = QuickDialog.DialogOption.Yes;
    if (!string.IsNullOrWhiteSpace(this.NegativeAnswer))
      dictionary[this.NegativeAnswer] = QuickDialog.DialogOption.No;
    if (!string.IsNullOrWhiteSpace(this.CancelContent))
      dictionary[this.CancelContent] = QuickDialog.DialogOption.Cancel;
    if (dictionary.Count == 0)
      throw new Exception("No dialog options specified.");
    QuickDialog.ShowMessageRequest showMessageRequest = new QuickDialog.ShowMessageRequest(this.Title, this.Question, (IEnumerable<string>) dictionary.Keys)
    {
      DontAskAgainEnabled = this.DontAskAgainEnabled
    };
    UserInput.Request((object) showMessageRequest, true);
    this.DontAskAgain = showMessageRequest.DontAskAgain;
    return dictionary[showMessageRequest.Response];
  }

  public enum DialogOption
  {
    Yes,
    No,
    Cancel,
  }

  private class ShowMessageRequest
  {
    public ShowMessageRequest(string title, string message, IEnumerable<string> options)
    {
      this.Name = title;
      this.Message = message;
      this.Options = options.ToArray<string>();
    }

    [Browsable(false)]
    public string[] Options { get; }

    [Browsable(false)]
    public string Name { get; }

    [Display("Message", null, null, 9.0, false, null)]
    [Layout]
    [Browsable(true)]
    public string Message { get; }

    [Browsable(false)]
    public bool DontAskAgainEnabled { get; set; }

    [Browsable(true)]
    [EnabledIf("DontAskAgainEnabled", new object[] {}, HideIfDisabled = true)]
    [Display("Don't ask again", null, null, 10.0, false, null)]
    public bool DontAskAgain { get; set; }

    [Layout]
    [Submit]
    [AvailableValues("Options")]
    [Browsable(true)]
    public string Response { get; set; }
  }
}
