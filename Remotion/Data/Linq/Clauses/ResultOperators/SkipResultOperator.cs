// This file is part of the re-motion Core Framework (www.re-motion.org)
// Copyright (C) 2005-2009 rubicon informationstechnologie gmbh, www.rubicon.eu
// 
// The re-motion Core Framework is free software; you can redistribute it 
// and/or modify it under the terms of the GNU Lesser General Public License 
// version 3.0 as published by the Free Software Foundation.
// 
// re-motion is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-motion; if not, see http://www.gnu.org/licenses.
// 
using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Data.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Data.Linq.Clauses.StreamedData;
using Remotion.Utilities;

namespace Remotion.Data.Linq.Clauses.ResultOperators
{
  /// <summary>
  /// Represents the skip part of a query. This is a result operator, operating on the whole result set of a query.
  /// </summary>
  /// <example>
  /// In C#, the "skip" clause in the following example corresponds to a <see cref="SkipResultOperator"/>.
  /// <code>
  /// var query = (from s in Students
  ///              select s).Skip(3);
  /// </code>
  /// </example>
  public class SkipResultOperator : SequenceTypePreservingResultOperatorBase
  {
    private Expression _count;

    public SkipResultOperator (Expression count)
    {
      ArgumentUtility.CheckNotNull ("count", count);
      Count = count;
    }

    public Expression Count
    {
      get { return _count; }
      set
      {
        ArgumentUtility.CheckNotNull ("value", value);
        if (value.Type != typeof (int))
        {
          var message = string.Format ("The value expression returns '{0}', an expression returning 'System.Int32' was expected.", value.Type);
          throw new ArgumentException (message, "value");
        }

        _count = value;
      }
    }

    /// <summary>
    /// Gets the constant <see cref="int"/> value of the <see cref="Count"/> property, assuming it is a <see cref="ConstantExpression"/>. If it is
    /// not, an expression is thrown.
    /// </summary>
    /// <returns>The constant <see cref="int"/> value of the <see cref="Count"/> property.</returns>
    public int GetConstantCount ()
    {
      var countAsConstantExpression = Count as ConstantExpression;
      if (countAsConstantExpression != null)
      {
        return (int) countAsConstantExpression.Value;
      }
      else
      {
        var message = string.Format (
            "Count ('{0}') is no ConstantExpression, it is a {1}.",
            FormattingExpressionTreeVisitor.Format (Count),
            Count.GetType ().Name);
        throw new InvalidOperationException (message);
      }
    }

    public override ResultOperatorBase Clone (CloneContext cloneContext)
    {
      return new SkipResultOperator (Count);
    }

    public override StreamedSequence ExecuteInMemory<T> (StreamedSequence input)
    {
      var sequence = input.GetTypedSequence<T> ();
      var result = sequence.Skip (GetConstantCount());
      return new StreamedSequence (result.AsQueryable (), (StreamedSequenceInfo) GetOutputDataInfo (input.DataInfo));
    }

    public override void TransformExpressions (Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull ("transformation", transformation);
      Count = transformation (Count);
    }

    public override string ToString ()
    {
      return "Skip(" + FormattingExpressionTreeVisitor.Format (Count) + ")";
    }
  }
}