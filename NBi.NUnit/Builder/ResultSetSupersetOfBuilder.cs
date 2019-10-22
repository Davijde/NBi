using System;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetBased.Comparison;

namespace NBi.NUnit.Builder
{
    class ResultSetSupersetOfBuilder : ResultSetEqualToBuilder
    {
        protected override EquivalenceKind EquivalenceKind
        {
            get { return EquivalenceKind.SupersetOf; }
        }

        public ResultSetSupersetOfBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SupersetOfXml))
                throw new ArgumentException("Constraint must be a 'SupersetOfXml'");

            ConstraintXml = (SupersetOfXml)ctrXml;
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IResultSetResolver resolver)
            => new SupersetOfConstraint(resolver);

    }
}
