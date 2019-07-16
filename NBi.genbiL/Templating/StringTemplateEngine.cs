﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Antlr4.StringTemplate;
using NBi.Xml;
using NBi.Xml.SerializationOption;

namespace NBi.GenbiL.Templating
{
    public class StringTemplateEngine
    {
        private readonly IDictionary<Type, XmlSerializer> cacheSerializer;
        private readonly IDictionary<Type, XmlSerializer> cacheDeserializer;

        public string TemplateXml { get; private set; }
        protected Template Template { get; private set; }
        public string PreProcessedTemplate { get; private set; }
        public string[] Variables { get; private set; }

        public StringTemplateEngine(string templateXml, string[] variables)
        {
            TemplateXml = templateXml;
            Variables = variables;
            cacheSerializer = new Dictionary<Type, XmlSerializer>();
            cacheDeserializer = new Dictionary<Type, XmlSerializer>();
        }

        public IEnumerable<TestXml> Build(List<List<List<object>>> table, IDictionary<string, object> globalVariables)
        {
            InitializeTemplate(globalVariables);
            var tests = new List<TestXml>();

            //For each row, we need to fill the variables and render the template. 
            int count = 0;
            foreach (var row in table)
            {
                count++;
                var str = BuildTestString(row);

                TestStandaloneXml test = null;
                try
                {
                    test = XmlDeserializeFromString<TestStandaloneXml>(str);
                }
                catch (InvalidOperationException ex)
                {
                    throw new TemplateExecutionException(ex.Message);
                }

                //Cleanup the variables in the template for next iteration.
                foreach (var variable in Variables)
                    Template.Remove(variable);

                test.Content = XmlSerializeFrom<TestStandaloneXml>(test);
                tests.Add(test);
                InvokeProgress(new ProgressEventArgs(count, table.Count()));
            }

            return tests;
        }

        internal void InitializeTemplate(IDictionary<string, object> globalVariables)
        {
            var group = new TemplateGroup('$', '$');
            group.RegisterRenderer(typeof(string), new StringRenderer());
            Template = new Template(group, TemplateXml);

            //Add all the global variables (not defined in a scope)
            if (globalVariables != null)
                foreach (var variable in globalVariables)
                    Template.Add(variable.Key, variable.Value);
        }

        internal string BuildTestString(List<List<object>> values)
        {
            for (int i = 0; i < Variables.Count(); i++)
            {
                // If the variable is not initialized or if it's value is "(none)" then we skip it.
                if (!(values[i].Count() == 0 || (values[i].Count == 1 && (values[i][0].ToString() == "(none)" || values[i][0].ToString() == string.Empty))))
                    Template.Add(Variables[i], values[i]);
                else
                    Template.Add(Variables[i], null);
            }

            var str = Template.Render();
            return str;
        }

        protected internal T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        protected internal string XmlSerializeFrom<T>(T objectData)
        {
            return SerializeFrom(objectData, typeof(T));
        }

        
        protected object XmlDeserializeFromString(string objectData, Type type)
        {
            if (!cacheDeserializer.ContainsKey(type))
            {
                var overrides = new ReadOnlyAttributes();
                overrides.Build();
                var builtDeserializer = new XmlSerializer(type, overrides);
                cacheDeserializer.Add(type, builtDeserializer);
            }

            var serializer = cacheDeserializer[type];
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        protected string SerializeFrom(object objectData, Type type)
        {
            if (!cacheSerializer.ContainsKey(type))
            {
                var overrides = new WriteOnlyAttributes();
                overrides.Build();
                var builtSerializer = new XmlSerializer(type, overrides);
                cacheSerializer.Add(type, builtSerializer);
            }

            var serializer = cacheSerializer[type];
            
            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                // Use the Serialize method to store the object's state.
                try
                {
                    serializer.Serialize(writer, objectData);
                }
                catch (Exception e)
                {

                    throw e;
                }

                result = writer.ToString();
            }
            return result;
        }


        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            Progressed?.Invoke(this, e);
        }

    }
}
