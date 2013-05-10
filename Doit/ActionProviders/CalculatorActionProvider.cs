using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Doit.ActionResults;
using Doit.Actions;
using NCalc;

namespace Doit.ActionProviders
{
	public class CalculatorActionProvider : SingleParameterActionProvider<CalculationAction>
	{
		public CalculatorActionProvider() : base("calc")
		{
		}

		public override ICollection<Type> CanConsume
		{
			get { return new[] { typeof(NumberResult) }; }
		}

		public override IEnumerable<CalculationAction> Offer(IAction action, string query)
		{
			var calculationAction = action as CalculationAction;

			return CalculateCore(calculationAction.Value.ToString(CultureInfo.InvariantCulture) + query);
		}

		protected override IEnumerable<CalculationAction> OfferCore(string parameter)
		{
			return CalculateCore(parameter);
		}

		private IEnumerable<CalculationAction> CalculateCore(string query)
		{
			try
			{
				var expression = new Expression(query, EvaluateOptions.IgnoreCase);
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