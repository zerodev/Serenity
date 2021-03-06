﻿using Serenity.Data;
using Serenity.Testing;
using Serenity.Test.Testing;
using System;
using Xunit;

namespace Serenity.Test.Data
{
    [Collection("AvoidParallel")]
    public partial class RowMappingTests : SerenityTestBase
    {
        [Fact]
        public void FaultyRowThrowsException()
        {
            Exception innerException = null;

            Assert.Throws(typeof(TypeInitializationException), () =>
            {
                try
                {
                    new FaultyRow();
                }
                catch (Exception ex)
                {
                    innerException = ex.InnerException;
                    throw;
                }
            });

            Assert.IsType(typeof(InvalidProgramException), innerException);
            Assert.Contains("Missing", innerException.Message);
        }

        [Fact]
        public void BasicRowHasCorrectMappings()
        {
            Assert.Equal("Basic", BasicRow.Fields.TableName);

            Assert.Equal("AString", BasicRow.Fields.AString.Name);
            Assert.Equal("T0.AString", BasicRow.Fields.AString.Expression);
            Assert.Equal("AString", BasicRow.Fields.AString.PropertyName);
            Assert.Equal(FieldFlags.Default, BasicRow.Fields.AString.Flags);
            Assert.Equal(0, BasicRow.Fields.AString.Size);
            Assert.Equal(0, BasicRow.Fields.AString.Scale);
            Assert.Equal(null, BasicRow.Fields.AString.DefaultValue);
            Assert.Equal(SelectLevel.Default, BasicRow.Fields.AString.MinSelectLevel);
            Assert.Equal(null, BasicRow.Fields.AString.ForeignTable);
            Assert.Equal(null, BasicRow.Fields.AString.ForeignField);
            Assert.Equal(null, BasicRow.Fields.AString.Join);
            Assert.Equal(null, BasicRow.Fields.AString.JoinAlias);
            Assert.Equal(null, BasicRow.Fields.AString.ReferencedAliases);
            Assert.Equal(null, BasicRow.Fields.AString.Origin);
            Assert.Equal(0, BasicRow.Fields.AString.NaturalOrder);
            Assert.Equal(null, BasicRow.Fields.AString.TextualField);
            Assert.Equal(null, BasicRow.Fields.AString.Caption);
            Assert.Equal("AString", BasicRow.Fields.AString.Title);
            Assert.Equal(FieldType.String, BasicRow.Fields.AString.Type);
            Assert.Equal(typeof(String), BasicRow.Fields.AString.ValueType);
            Assert.Equal(0, BasicRow.Fields.AString.Index);
            Assert.Equal(BasicRow.Fields, BasicRow.Fields.AString.Fields);

            Assert.Equal("AInt32", BasicRow.Fields.AInt32.Name);
            Assert.Equal("T0.AInt32", BasicRow.Fields.AInt32.Expression);
            Assert.Equal("AInt32", BasicRow.Fields.AInt32.PropertyName);
            Assert.Equal(FieldFlags.Default, BasicRow.Fields.AInt32.Flags);
            Assert.Equal(0, BasicRow.Fields.AInt32.Size);
            Assert.Equal(0, BasicRow.Fields.AInt32.Scale);
            Assert.Equal(null, BasicRow.Fields.AInt32.DefaultValue);
            Assert.Equal(SelectLevel.Default, BasicRow.Fields.AInt32.MinSelectLevel);
            Assert.Equal(null, BasicRow.Fields.AInt32.ForeignTable);
            Assert.Equal(null, BasicRow.Fields.AInt32.ForeignField);
            Assert.Equal(null, BasicRow.Fields.AInt32.Join);
            Assert.Equal(null, BasicRow.Fields.AInt32.JoinAlias);
            Assert.Equal(null, BasicRow.Fields.AInt32.ReferencedAliases);
            Assert.Equal(null, BasicRow.Fields.AInt32.Origin);
            Assert.Equal(0, BasicRow.Fields.AInt32.NaturalOrder);
            Assert.Equal(null, BasicRow.Fields.AInt32.TextualField);
            Assert.Equal(null, BasicRow.Fields.AInt32.Caption);
            Assert.Equal("AInt32", BasicRow.Fields.AInt32.Title);
            Assert.Equal(FieldType.Int32, BasicRow.Fields.AInt32.Type);
            Assert.Equal(typeof(Int32?), BasicRow.Fields.AInt32.ValueType);
            Assert.Equal(1, BasicRow.Fields.AInt32.Index);
            Assert.Equal(BasicRow.Fields, BasicRow.Fields.AInt32.Fields);

            
        }

        [Fact]
        public void TableNameAttributeSetsRowTableName()
        {
            Assert.Equal("ComplexTable", ComplexRow.Fields.TableName);
        }

        [Fact]
        public void DisplayNameAttributeIsHonored()
        {
            Assert.Equal("Complex ID", ComplexRow.Fields.ID.Title);
        }

        [Fact]
        public void DisplayNameAttributeOverridesManualValue()
        {
            Assert.Equal("OverridenCaption", ComplexRow.Fields.Overriden.Title);
        }

        [Fact]
        public void ColumnAttributeCantOverrideManualValue()
        {
            Exception innerException = null;

            Assert.Throws(typeof(TypeInitializationException), () =>
            {
                try
                {
                    new InvalidColumnRow();
                }
                catch (Exception ex)
                {
                    innerException = ex.InnerException;
                    throw;
                }
            });

            Assert.IsType(typeof(InvalidProgramException), innerException);
            Assert.Contains("can't be overridden", innerException.Message);
            Assert.Contains("'Override'", innerException.Message);
        }

        [Fact]
        public void ColumnAttributeOverrideCanUseSameNameWithManualValue()
        {
            Assert.Equal("ManualName", ComplexRow.Fields.Overriden.Name);
        }

        [Fact]
        public void ExpressionAttributeOverridesManualValue()
        {
            Assert.Equal("T0.OverridenExpression", ComplexRow.Fields.Overriden.Expression);
        }

        [Fact]
        public void ForeignKeyAttributeSetsPropertiesProperly()
        {
            Assert.Equal("TheCountryTable", ComplexRow.Fields.CountryID.ForeignTable);
            Assert.Equal("TheCountryID", ComplexRow.Fields.CountryID.ForeignField);
        }

        [Fact]
        public void AddJoinAttributeCreatesLeftJoinProperly()
        {
            Assert.True(ComplexRow.Fields.Joins.ContainsKey("c"));

            var join = ComplexRow.Fields.Joins["c"];
            Assert.Equal(
                TestSqlHelper.Normalize("c.TheCountryID = T0.CountryID"),
                TestSqlHelper.Normalize(join.OnCriteria.ToStringIgnoreParams()));

            Assert.Equal("TheCountryTable", join.Table);

            Assert.IsType(typeof(LeftJoin), join);
        }

        [Fact]
        public void ExpressionAttributeSetsForeignFlagIfContainsAnyAliasOtherThanT0()
        {
            Assert.True(ComplexRow.Fields.CountryName.Flags.HasFlag(FieldFlags.Foreign));
        }

        [Fact]
        public void ExpressionAttributeDoesntSetForeignFlagIfContainsOnlyT0()
        {
            Assert.False(ComplexRow.Fields.Name.Flags.HasFlag(FieldFlags.Foreign));
        }

        [Fact]
        public void ExpressionAttributeDoesntSetCalculatedFlagIfContainsOnlyOneAliasAndName()
        {
            Assert.False(ComplexRow.Fields.Name.Flags.HasFlag(FieldFlags.Calculated));
            Assert.False(ComplexRow.Fields.CountryName.Flags.HasFlag(FieldFlags.Calculated));
        }

        [Fact]
        public void ExpressionAttributeSetsCalculatedFlagIfContainsMoreThanOneAliasAndName()
        {
            Assert.True(ComplexRow.Fields.FullName.Flags.HasFlag(FieldFlags.Calculated));
        }
    }
}