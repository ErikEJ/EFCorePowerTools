namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// File supplied to template file service.
    /// </summary>
    public class InputFile
    {
        /// <summary>
        /// Directory name.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// File contents.
        /// </summary>
        public string Contents { get; set; }
    }
}