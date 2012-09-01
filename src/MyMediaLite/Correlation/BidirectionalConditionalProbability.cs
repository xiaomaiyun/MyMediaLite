// Copyright (C) 2011, 2012 Zeno Gantner
// Copyright (C) 2010 Steffen Rendle, Zeno Gantner
//
// This file is part of MyMediaLite.
//
// MyMediaLite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MyMediaLite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MyMediaLite.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using MyMediaLite.DataType;

namespace MyMediaLite.Correlation
{
	/// <summary>Class for storing and 'bi-directional' computing conditional probabilities</summary>
	/// <remarks>
	/// TODO LIT
	/// </remarks>
	///
	public sealed class BidirectionalConditionalProbability : BinaryDataAsymmetricCorrelationMatrix
	{
		float Alpha { get; set; } // TODO check value

		/// <summary>Creates an object of type BidirectionalConditionalProbability</summary>
		/// <param name="num_entities">the number of entities</param>
		/// <param name="alpha">alpha parameter</param>
		public BidirectionalConditionalProbability(int num_entities, float alpha) : base(num_entities)
		{
			Alpha = alpha;
		}

		/// <summary>Creates conditional probability matrix from given data</summary>
		/// <param name="vectors">the boolean data</param>
		/// <param name="alpha">alpha parameter</param>
		/// <returns>the similarity matrix based on the data</returns>
		static public BidirectionalConditionalProbability Create(IBooleanMatrix vectors, float alpha)
		{
			BidirectionalConditionalProbability cm;
			int num_entities = vectors.NumberOfRows;
			try
			{
				cm = new BidirectionalConditionalProbability(num_entities, alpha);
			}
			catch (OverflowException)
			{
				Console.Error.WriteLine("Too many entities: " + num_entities);
				throw;
			}
			cm.ComputeCorrelations(vectors);
			return cm;
		}

		///
		protected override float ComputeCorrelationFromOverlap(uint overlap, int count_x, int count_y)
		{
			if (count_x == 0 || count_y == 0)
				return 0.0f;
			
			double x_given_y = (double) (overlap / count_x);
			double y_given_x = (double) (overlap / count_y);
			
			return (float) ( Math.Pow(x_given_y, Alpha) * Math.Pow(y_given_x, 1 - Alpha) );
		}
	}
}