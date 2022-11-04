
namespace Acidmanic.Utilities.Results
{
    public class Result<T>:Result
    {
        public T Value { get; set; }

        public Result()
        {
            Success = false;
            Value = default;
        }

        public Result(bool success, T value)
        {
            this.Success = success;
            this.Value = value;
        }

        public Result<T> FailAndDefaultValue()
        {
            Success = false;

            this.Value = default;
            
            return this;
        }

        public Result<T> Succeed(T value)
        {
            Value = value;
            Success = true;
            return this;
        }


        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(true, value);
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }

    }
}