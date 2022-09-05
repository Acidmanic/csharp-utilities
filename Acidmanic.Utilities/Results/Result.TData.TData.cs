namespace Acidmanic.Utilities.Results
{
    public class Result<T1, T2>:Result
    {
        public T2 Secondary { get; set; }
        public T1 Primary { get; set; }

        public Result()
        {
            Success = false;
            Primary = default;
            Secondary = default;
        }

        public Result(bool success, T2 secondary, T1 primary)
        {
            this.Success = success;
            this.Secondary = secondary;
            this.Primary = primary;
        }

        public Result<T1, T2> FailAndDefaultBothValues()
        {
            this.Success = false;

            Primary = default;

            Secondary = default;
            
            return this;
        }

        public Result<T1, T2> Succeed(T1 primary, T2 secondary)
        {
            this.Success = true;
            this.Primary = primary;
            this.Secondary = secondary;
            return this;
        }
        

        public static implicit operator T1(Result<T1,T2> result)
        {
            return result.Primary;
        }
        
        public static implicit operator T2(Result<T1,T2> result)
        {
            return result.Secondary;
        }
    }
}