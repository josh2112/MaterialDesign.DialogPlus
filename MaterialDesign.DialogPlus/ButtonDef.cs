namespace Com.Josh2112.Libs.MaterialDesign.DialogPlus
{
    /// <summary>
    /// Represents a traditional UI button. This class is meant to provide an easy
    /// way to define the button configuration of a dialog or a prompt. A button
    /// can have a "connotation" of positive, neutral, or negative. It's up to the
    /// specific app or UI how to visiaully represent these connotations.
    ///
    /// The constructor is hidden from the outside world. To create a
    /// button, use of the factory methods Negative(), Positive(),
    /// or Neutral(), passing it the button text.
    /// </summary>
    public class ButtonDef
    {
        public enum Connotations
        {
            Negative,
            Positive,
            Neutral
        }

        /// <summary>
        /// The button label
        /// </summary>
        public string Text { get; private set; }

        public Connotations Connotation { get; private set; }

        /// <summary>
        /// Makes this button respond to the [Enter] key
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Makes this button respond to the [Esc] key
        /// </summary>
        public bool IsCancel { get; set; }

        private ButtonDef( string text, Connotations type ) { Text = text; Connotation = type; }

        public static ButtonDef Negative( string text ) => new ButtonDef( text, Connotations.Negative );

        public static ButtonDef Positive( string text, bool isDefault = false, bool isCancel = false ) =>
            new ButtonDef( text, Connotations.Positive ) { IsDefault = isDefault, IsCancel = isCancel };

        public static ButtonDef Neutral( string text, bool isDefault = false, bool isCancel = false ) =>
            new ButtonDef( text, Connotations.Neutral ) { IsDefault = isDefault, IsCancel = isCancel };
    }
}
