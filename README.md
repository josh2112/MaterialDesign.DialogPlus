# MaterialDesign.DialogPlus

![Screenshot of a basic dialog](web/images/example-basic-dialog.png)

MaterialDesign.DialogPlus is a small package to make working with dialogs a bit easier in the excellent [MaterialDesignInXaml](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit). It consists of only a few small files, has no dependencies (besides MaterialDesignInXaml!) and supports .NET 7.0 all the way back to .NET Framework 4.6.2.

MaterialDesignInXaml has great support for dialogs, but it's not clear how to use multiple dialogs with its `DialogHost`. This package provides the following components to make your dialog experience more fun:

 - `IHasDialogResult<T>`: an interface for a cancelable dialog with genericised result
 - `ShowDialogForResultAsync<T>`: a wrapper method to show a dialog and wait for its response
 - `Dialog`: a generic dialog imlpementation with title, message and configurable buttons which should suit 99% of your dialog use-cases.
 - `ButtonDef`: A simple model for dialog buttons. Includes a label, a "connotation" (Positive, Negative, or Neutral) which determines how the button is drawn, and a couple properties to make the button activate on [Enter] or [Esc].

## Basic dialog

Here's the code for the above screenshot:
```
ButtonDef result = await this.ShowDialogForResultAsync( new Dialogs.Dialog( "Question",
    "Your confrobulator has unsaved changes. What do you want to do?",
    ButtonDef.Neutral( "Keep working", isCancel: true ),
    ButtonDef.Negative( "Delete it" ),
    ButtonDef.Positive( "Save it", isDefault: true ) ) );
```

It uses the built-in, generic `Dialog` class to show a title, message and three buttons. The `Dialog` class implements `IHasDialog<ButtonDef>`, meaning the result will be one of the buttons you passed in. If you write your own dialog, you can return anything you like.

The built-in dialog requires a couple custom styles. If you want to use it, make sure you put the following in your `App.xaml` to import them:
```
<ResourceDictionary Source="pack://application:,,,/MaterialDesign.DialogPlus;component/Themes/Defaults.xaml" />
```

### Basic dialog: Button connotations and restyling buttons

When defining buttons for the built-in dialog, you have 3 `Connotations`:
 - `Connotations.Neutral`: Used for an action with no side effects, like "Cancel" or "Continue". The style for this is `MaterialDesign.DialogPlus.NeutralButton` which is an outlined button with the primary theme color.
 - `Connotations.Positive`: Used for positive actions, such as acknowledging a message or saving a file. The style for this is `MaterialDesign.DialogPlus.PositiveButton` which is a raised button with the primary theme color.
 - `Connotations.Negative`: Used for destructive actions, such as discarding changes or deleting a message. The style for this is `MaterialDesign.DialogPlus.NegativeButton` which is an outlined button with the outline and text color set to `MaterialDesignValidationErrorBrush` which is reddish in color.

You can change how any of these buttons are rendered in the built-in `Dialog` by defining your own style with the same name. Just put this in your `App.xaml` after you import the default button styles. For example:
```
<Style x:Key="MaterialDesign.DialogPlus.NegativeButton" TargetType="Button" BasedOn="{StaticResource MaterialDesignRaisedButton}">
    <Setter Property="Background" Value="Salmon"/>
    <Setter Property="BorderBrush" Value="Salmon"/>
</Style>
```

Now buttons created with `ButtonDef.Negative()` will have a salmon-colored border and background.

## Custom dialog

The built-in `Dialog` class should suit 99% of use cases. But you can build a completely custom dialog if you like.

1. Create a `UserControl`. Write the dialog XAML and the code-behind however you like.
2. Make the `UserControl` implement `IHasDialogResult<T>` with the desired result type. For example, if your dialog is a text input, you might implement `IHasDialogResult<string>`.
3. To close your dialog, call `Result.Set` with the dialog result (for the text input example, this might be the user-input text, or `null` if the user canceled the dialog).

Check out the [main file](MaterialDesign.DialogPlus.Examples/MainWindow.xaml.cs) of the example project for the following examples:

1. The basic dialog shown in the first screenshot.
2. Canceling a dialog with a CancelToken.
3. A custom input dialog with multiple inputs.
