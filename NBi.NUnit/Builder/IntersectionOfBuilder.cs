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
    class IntersectionOfBuilder : ResultSetEqualToBuilder
    {
        protected override EquivalenceKind EquivalenceKind  { get => EquivalenceKind.IntersectionOf; }

        public IntersectionOfBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is IntersectionOfXml))
                throw new ArgumentException("Constraint must be a 'ResultSetEquivalentToXml'");

            ConstraintXml = (IntersectionOfXml)ctrXml;
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IResultSetResolver resolver)
            => new IntersectionOfConstraint(resolver);

    }
}
