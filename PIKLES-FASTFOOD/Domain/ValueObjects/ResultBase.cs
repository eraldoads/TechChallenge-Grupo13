using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class ResultBase
    {
        public enum ResultType
        {
            Accepted,
            BadRequest,
            Created,
            Custom,
            Exception,
            NotFound,
            NoContent,
            PreCondition,
            PartialContent,
            PermissionDenied,
            Success,
            Unauthorized
        }

        public class ErrorValidacao
        {
            public string MensagemErro { get; set; }

            public List<ResultError> ListaErros { get; set; }
        }

        public class ResultError
        {
            public string MensagemErro { get; set; }

            public string CampoErro { get; set; }
        }

        public abstract class Result<T>
        {
            public abstract ResultType ResultType { get; }

            public ErrorValidacao Error { get; set; }

            public string MensagemError { get; set; }

            public int? Code { get; set; }

            public T Data { get; set; }
        }

        public class PagedResult<T> : Result<IList<T>>
        {
            public int _Offset { get; private set; }

            public int _Limit { get; private set; }

            public int _Total { get; private set; }

            public override ResultType ResultType
            {
                get
                {
                    if (_Total == 0 || base.Data.Count == 0)
                    {
                        return ResultType.NoContent;
                    }

                    if (_Total > base.Data.Count)
                    {
                        return ResultType.PartialContent;
                    }

                    return ResultType.Success;
                }
            }

            public PagedResult(int offset, int limit, int total, IList<T> list = null)
            {
                if (list == null)
                {
                    GeneratePagedResultCustom(list);
                    return;
                }

                _Offset = offset;
                _Limit = limit;
                _Total = total;
                base.Data = list;
            }

            public PagedResult(IList<T> list = null)
            {
                GeneratePagedResultCustom(list);
            }

            private void GeneratePagedResultCustom(IList<T> list)
            {
                _Offset = 0;
                _Total = list?.Count ?? 0;
                _Limit = _Total;
                base.Data = list;
            }
        }
    }
}
