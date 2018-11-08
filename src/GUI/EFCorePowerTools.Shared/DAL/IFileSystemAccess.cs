namespace EFCorePowerTools.Shared.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IFileSystemAccess
    {
        Version GetInstalledSqlCe40Version();

        /// <summary>
        /// Writes the <paramref name="lines"/> to the <paramref name="fileName"/> using the <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="fileName">The path of the file to write to.</param>
        /// <param name="lines">The lines to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> or <paramref name="lines"/> are <b>null</b>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> is empty or contains only white spaces.</exception>
        void WriteAllLines(string fileName,
                           IEnumerable<string> lines);

        /// <summary>
        /// Reads all lines from the <paramref name="fileName"/> using the <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="fileName">The path of the file to read from.</param>
        /// <returns>An array of all lines in the file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is <b>null</b>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> is empty or contains only white spaces.</exception>
        string[] ReadAllLines(string fileName);
    }
}