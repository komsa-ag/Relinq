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
using System.Reflection;
using Remotion.Data.Linq.Clauses;
using Remotion.Data.Linq.Clauses.ResultModifications;
using Remotion.Utilities;

namespace Remotion.Data.Linq.Parsing.Structure.IntermediateModel
{
  /// <summary>
  /// Represents a <see cref="MethodCallExpression"/> for <see cref="Queryable.Take{TSource}"/>.
  /// It is generated by <see cref="ExpressionTreeParser"/> when an <see cref="Expression"/> tree is parsed.
  /// When this node is used, it usually follows (or replaces) a <see cref="SelectExpressionNode"/> of an <see cref="IExpressionNode"/> chain that 
  /// represents a query.
  /// </summary>
  public class TakeExpressionNode : MethodCallExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new[]
                                                           {
                                                               GetSupportedMethod (() => Queryable.Take<object> (null, 0))
                                                           };

    public TakeExpressionNode (IExpressionNode source, int count)
      : base (ArgumentUtility.CheckNotNull ("source", source))
    {
      Count = count;
    }

    public int Count { get; set; }

    public override Expression Resolve (ParameterExpression inputParameter, Expression expressionToBeResolved)
    {
      // this simply streams its input data to the output without modifying its structure, so we resolve by passing on the data to the previous node
      return Source.Resolve (inputParameter, expressionToBeResolved);
    }

    public override ParameterExpression CreateParameterForOutput ()
    {
      // this simply streams its input data to the output without modifying its structure, so we let the previous node create the parameter
      return Source.CreateParameterForOutput ();
    }

    public override IClause CreateClause (IClause previousClause)
    {
      ArgumentUtility.CheckNotNull ("previousClause", previousClause);

      var selectClause = GetSelectClauseForResultModification (previousClause);
      selectClause.AddResultModification (new TakeResultModification (selectClause, Count));

      return selectClause;
    }
  }
}