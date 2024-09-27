using System.ComponentModel.DataAnnotations;

namespace AppMoney.Database.Validation
{
    /// <summary>
    /// Composite validation result.
    /// </summary>
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        public IEnumerable<ValidationResult> Results => _results;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YoiPos.Attributes.CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        public CompositeValidationResult(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YoiPos.Attributes.CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="memberNames">Member names.</param>
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames)
            : base(errorMessage, memberNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YoiPos.Attributes.CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        protected CompositeValidationResult(ValidationResult validationResult)
            : base(validationResult)
        {
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        public void AddResult(ValidationResult validationResult) => _results.Add(validationResult);
    }
}
