using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGen.SUGAR.Contracts
{
	public class EvaluationDatasResponse : Response
	{
		public EvaluationDataResponse[] Items { get; set; }
	}
}
