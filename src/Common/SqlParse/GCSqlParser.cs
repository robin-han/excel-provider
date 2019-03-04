using Antlr4.Runtime;
using GrapeCity.Enterprise.Data.DataSource.Common.Expression;

namespace GrapeCity.Enterprise.Data.DataSource.Common.SqlParse
{
    public class GCSqlParser
    {
        private ISQLStatement _statement;
        private string _sql;

        public GCSqlParser()
        {
        }

        internal ISQLStatement InternalParse(string sql)
        {
            var inputStream = new AntlrInputStream(sql);
            var lexer = new SqlLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SqlParser(tokens);
            var visitor = new GCSqlVisitor(parser,sql);
            return (ISQLStatement)visitor.Visit(parser.parse());
        }

        public ISQLStatement Parse(string sql)
        {
            if(sql == _sql && _statement!=null)
            {
                return _statement;
            }

            _sql = sql;
            _statement = InternalParse(sql);
            return _statement;
        }
    }
}
