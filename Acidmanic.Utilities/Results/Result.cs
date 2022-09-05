namespace Acidmanic.Utilities.Results
{
    public  class Result
    {
        protected bool Equals(Result other)
        {
            return Success == other.Success;
        }
        
        public bool Success { get; set; }

        public Result Succeed()
        {
            Success = true;

            return this;
        }
        
        public Result Fail()
        {
            Success = false;

            return this;
        }
        
        public static bool operator ==(Result value, bool bValue)
        {
            return value?.Success == bValue;
        }

        public static bool operator !=(Result value, bool bValue)
        {
            return !(value == bValue);
        }
        
        public static implicit operator bool(Result r) => r.Success;
        
        public static implicit operator Result(bool success) => new Result{Success = success};
    }
}