using System;
using System.Security;
using System.Xml;

namespace RevEng.Core.Diagram
{
    internal sealed class DgmlHelper : IDisposable
    {
        private readonly XmlTextWriter xtw;

        internal DgmlHelper(string outputFile)
        {
            xtw = new XmlTextWriter(outputFile, System.Text.Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;
            xtw.WriteStartDocument();
            xtw.WriteStartElement("DirectedGraph", "http://schemas.microsoft.com/vs/2009/dgml");
            xtw.WriteAttributeString("GraphDirection", "LeftToRight");
        }

        public void Dispose()
        {
            if (xtw != null)
            {
                xtw.Close();
            }
        }

        internal void WriteNode(string id, string label)
        {
            xtw.WriteAttributeString("Id", SecurityElement.Escape(id));
            xtw.WriteAttributeString("Label", SecurityElement.Escape(label));

            xtw.WriteEndElement();
        }

        internal void WriteNode(string id, string label, string reference, string category, string group, string description)
        {
            xtw.WriteStartElement("Node");

#pragma warning disable SA1503 // Braces should not be omitted
            xtw.WriteAttributeString("Id", SecurityElement.Escape(id));
            if (!string.IsNullOrEmpty(label))
                xtw.WriteAttributeString("Label", SecurityElement.Escape(label));
            if (!string.IsNullOrEmpty(reference))
                xtw.WriteAttributeString("Reference", SecurityElement.Escape(reference));
            if (!string.IsNullOrEmpty(category))
                xtw.WriteAttributeString("Category", SecurityElement.Escape(category));
            if (!string.IsNullOrEmpty(group))
                xtw.WriteAttributeString("Group", SecurityElement.Escape(group));
            if (!string.IsNullOrEmpty(description))
                xtw.WriteAttributeString("Description", SecurityElement.Escape(description));

            xtw.WriteEndElement();
        }

        internal void WriteLink(string source, string target, string label, string category)
        {
            xtw.WriteStartElement("Link");

            xtw.WriteAttributeString("Source", SecurityElement.Escape(source));
            xtw.WriteAttributeString("Target", SecurityElement.Escape(target));
            if (!string.IsNullOrEmpty(label))
                xtw.WriteAttributeString("Label", SecurityElement.Escape(label));
            if (!string.IsNullOrEmpty(category))
                xtw.WriteAttributeString("Category", SecurityElement.Escape(category));

            xtw.WriteEndElement();
        }

        internal void BeginElement(string element)
        {
            xtw.WriteStartElement(SecurityElement.Escape(element));
        }

        internal void EndElement()
        {
            xtw.WriteEndElement();
        }

        internal void Close()
        {
            xtw.WriteStartElement("Styles");

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "Table");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('Table')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FFC0C0C0");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");

            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "Schema");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('Schema')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FF7F9169");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "PK");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('PK')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FF008000");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "NULL");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('NULL')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FF808040");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "FK");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('FK')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FF8080FF");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "NOT NULL");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('NOT NULL')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FFC0A000");
            xtw.WriteEndElement();

            xtw.WriteEndElement();

            xtw.WriteStartElement("Style");
            xtw.WriteAttributeString("TargetType", "Node");
            xtw.WriteAttributeString("GroupLabel", "Database");
            xtw.WriteAttributeString("ValueLabel", "True");

            xtw.WriteStartElement("Condition");
            xtw.WriteAttributeString("Expression", "HasCategory('Database')");
            xtw.WriteEndElement();

            xtw.WriteStartElement("Setter");
            xtw.WriteAttributeString("Property", "Background");
            xtw.WriteAttributeString("Value", "#FFFFFFFF");
            xtw.WriteEndElement();

            xtw.WriteEndElement();
            xtw.WriteEndElement();

            xtw.WriteEndElement();
            xtw.Close();
        }
#pragma warning restore SA1503 // Braces should not be omitted

    }
}
