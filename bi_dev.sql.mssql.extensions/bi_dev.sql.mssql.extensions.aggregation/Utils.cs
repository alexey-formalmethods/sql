using MathNet.Numerics.Statistics;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.aggregation.Utils
{
    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined, //use clr serialization to serialize the intermediate result  
        IsInvariantToNulls = true, //optimizer property  
        IsInvariantToDuplicates = false, //optimizer property  
        IsInvariantToOrder = false, //optimizer property  
        MaxByteSize = 8000
    )] //maximum size in bytes of persisted value  
    public class StringAgg: IBinarySerialize
    {
        /// <summary>  
        /// The variable that holds the intermediate result of the concatenation  
        /// </summary>  
        public StringBuilder intermediateResult;

        /// <summary>  
        /// Initialize the internal data structures  
        /// </summary>  
        public void Init()
        {
            this.intermediateResult = new StringBuilder();
        }

        /// <summary>  
        /// Accumulate the next value, not if the value is null  
        /// </summary>  
        /// <param name="value"></param>  
        public void Accumulate(SqlString value, SqlString delimiter)
        {
            if (value.IsNull)
            {
                return;
            }
            this.intermediateResult.Append(value.Value).Append(delimiter);
        }

        /// <summary>  
        /// Merge the partially computed aggregate with this aggregate.  
        /// </summary>  
        /// <param name="other"></param>  
        public void Merge(StringAgg other)
        {
            this.intermediateResult.Append(other.intermediateResult);
        }

        /// <summary>  
        /// Called at the end of aggregation, to return the results of the aggregation.  
        /// </summary>  
        /// <returns></returns>  
        public SqlString Terminate()
        {
            string output = string.Empty;
            //delete the trailing comma, if any  
            if (this.intermediateResult != null
                && this.intermediateResult.Length > 0)
            {
                output = this.intermediateResult.ToString(0, this.intermediateResult.Length - 1);
            }

            return new SqlString(output);
        }

        public void Read(BinaryReader r)
        {
            intermediateResult = new StringBuilder(r.ReadString());
        }

        public void Write(BinaryWriter w)
        {
            w.Write(this.intermediateResult.ToString());
        }
    }
    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined, //use clr serialization to serialize the intermediate result  
        IsInvariantToNulls = true, //optimizer property  
        IsInvariantToDuplicates = false, //optimizer property  
        IsInvariantToOrder = false, //optimizer property  
        MaxByteSize = -1
    )] //maximum size in bytes of persisted value  
    public class ToJsonArray : IBinarySerialize
    {
        /// <summary>  
        /// The variable that holds the intermediate result of the concatenation  
        /// </summary>  
        public StringBuilder intermediateResult = new StringBuilder();
        
        /// <summary>  
        /// Initialize the internal data structures  
        /// </summary>  
        public void Init()
        {
            intermediateResult = new StringBuilder();
        }

        /// <summary>  
        /// Accumulate the next value, not if the value is null  
        /// </summary>  
        /// <param name="value"></param>  
        public void Accumulate(SqlString value)
        {
            
        }

        /// <summary>  
        /// Merge the partially computed aggregate with this aggregate.  
        /// </summary>  
        /// <param name="other"></param>  
        public void Merge(ToJsonArray other)
        {
            
        }

        /// <summary>  
        /// Called at the end of aggregation, to return the results of the aggregation.  
        /// </summary>  
        /// <returns></returns>  
        public SqlString Terminate()
        {
            
            return new SqlString(intermediateResult.ToString());
        }
        public void Read(BinaryReader r)
        {
            intermediateResult.Append(r.ReadString());
            
        }
        public void Write(BinaryWriter w)
        {
            w.Write(this.intermediateResult.ToString());
        }
    }
    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined, //use clr serialization to serialize the intermediate result  
        IsInvariantToNulls = true, //optimizer property  
        IsInvariantToDuplicates = true, //optimizer property  
        IsInvariantToOrder = true, //optimizer property  
        MaxByteSize = -1
    )] //maximum size in bytes of persisted value  
    public struct Median: IBinarySerialize
    {
        /// <summary>  
        /// The variable that holds the intermediate result of the concatenation  
        /// </summary>  
        public List<double?> values;

        /// <summary>  
        /// Initialize the internal data structures  
        /// </summary>  
        public void Init()
        {
            values = new List<double?>();
        }

        /// <summary>  
        /// Accumulate the next value, not if the value is null  
        /// </summary>  
        /// <param name="value"></param>  
        public void Accumulate(SqlDouble value, bool isNullEqualToZero)
        {
            if(isNullEqualToZero && value.IsNull)
            {
                value = 0;
            }
            if (!value.IsNull)
            {
                this.values.Add(value.Value);
            }
        }

        /// <summary>  
        /// Merge the partially computed aggregate with this aggregate.  
        /// </summary>  
        /// <param name="other"></param>  
        public void Merge(Median other)
        {
            this.values.AddRange(other.values);
        }

        /// <summary>  
        /// Called at the end of aggregation, to return the results of the aggregation.  
        /// </summary>  
        /// <returns></returns>  
        public double? Terminate()
        {
            return values.Median();
        }
        public void Read(System.IO.BinaryReader r)
        {
            while(r.BaseStream.Position != r.BaseStream.Length) 
            { 
                this.values.Add(r.ReadDouble());
            }
        }

        public void Write(System.IO.BinaryWriter w)
        {
            foreach (double d in this.values)
            {
                w.Write(d);
            }
        }


    }

}
