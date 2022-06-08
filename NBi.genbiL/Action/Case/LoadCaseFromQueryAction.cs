using System;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.Extensibility.Resolving;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromQueryAction : ISingleCaseAction
    {
        public string Query { get; set; }
        public IScalarResolver ConnectionString { get; set; }

        protected LoadCaseFromQueryAction() { }

        public LoadCaseFromQueryAction(string query, IScalarResolver connectionString)
        {
            Query = query;
            ConnectionString = connectionString;
        }

        public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

        public virtual void Execute(CaseSet testCases)
        {
            var queryEngineFactory = new ExecutionEngineFactory();
            var queryEngine = queryEngineFactory.Instantiate(new Query(Query, ConnectionString.Execute().ToString()));
            var ds = queryEngine.Execute();
            testCases.Content = ds.Tables[0];
        }

        public string Display => $"Loading TestCases from query '{Query}'";
    }
}
