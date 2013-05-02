using System;
using System.Collections.Generic;
using System.Linq;
using Doit.Actions;
using NCalc;

namespace Doit.ActionProviders
{
	public class CalculatorActionProvider : SingleParameterActionProvider<CalculationAction>
	{
		public CalculatorActionProvider() : base("calc")
		{
		}

		protected override IEnumerable<CalculationAction> OfferCore(string parameter)
		{
			try
			{
				var expression = new Expression(parameter, EvaluateOptions.IgnoreCase);
				var result = expression.Evaluate();

				double value;

				if (result is int)
				{
					value = (int)result;
				}
				else if (result is double)
				{
					value = (double)result;
				}
				else if (result is decimal)
				{
					value = (double)(decimal)result;
				}
				else
				{
					return Enumerable.Empty<CalculationAction>();
				}

				return new[] { new CalculationAction(value) { Hint = string.Format("Result of {0}", expression.ParsedExpression) } };
			}
			catch (Exception)
			{
			}

			return Enumerable.Empty<CalculationAction>();
		}
	}
}