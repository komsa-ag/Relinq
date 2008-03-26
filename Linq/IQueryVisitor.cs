using Rubicon.Data.Linq.Clauses;

namespace Rubicon.Data.Linq
{
  public interface IQueryVisitor
  {
    void VisitQueryExpression (QueryExpression queryExpression);
    void VisitMainFromClause (MainFromClause fromClause);
    void VisitAdditionalFromClause (AdditionalFromClause fromClause);
    void VisitSubQueryFromClause (SubQueryFromClause clause);
    void VisitJoinClause (JoinClause joinClause);
    void VisitLetClause (LetClause letClause);
    void VisitWhereClause (WhereClause whereClause);
    void VisitOrderByClause (OrderByClause orderByClause);
    void VisitOrderingClause (OrderingClause orderingClause);
    void VisitSelectClause (SelectClause selectClause);
    void VisitGroupClause (GroupClause groupClause);
  }
}