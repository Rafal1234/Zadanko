using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadanko.Services;
using Zadanko.ViewModels;

namespace ZadankoTests.ViewModels
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiService> _apiServiceMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiServiceMock = new Mock<IApiService>();
            _viewModel = new MainWindowViewModel(_apiServiceMock.Object);
        }

        [Fact]
        public async Task Call_AddNumbers_WithValidInputst()
        {
            // Arrange
            _apiServiceMock.Setup(x => x.AddNumbersAsync(1, 3))
                .ReturnsAsync(4);
            _viewModel.FirstNumberInput = "1";
            _viewModel.SecondNumberInput = "3";

            // Act
            await _viewModel.AddNumbersCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(4, _viewModel.Result);
            _apiServiceMock.Verify(x => x.AddNumbersAsync(1, 3), Times.Once);
            Assert.False(_viewModel.IsBusy);
        }

        [Fact]
        public async Task Call_AddNumbers_WithInvalidInputs()
        {
            // Arrange
            _viewModel.FirstNumberInput = "abc";
            _viewModel.SecondNumberInput = "3";

            // Act
            await _viewModel.AddNumbersCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(0, _viewModel.Result);
            _apiServiceMock.Verify(x => x.AddNumbersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Assert.False(_viewModel.IsBusy);
        }

        [Fact]
        public async Task Call_AddNumbers_WhenBusy()
        {
            // Arrange
            _viewModel.IsBusy = true;
            _viewModel.FirstNumberInput = "6";
            _viewModel.SecondNumberInput = "2";

            // Act
            await _viewModel.AddNumbersCommand.ExecuteAsync(null);

            // Assert
            _apiServiceMock.Verify(x => x.AddNumbersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("12.34")]
        [InlineData("!@#")]
        public void Set_FirstNumber_WithInvalidInput(string invalidInput)
        {
            // Act
            _viewModel.FirstNumberInput = invalidInput;

            // Assert
            Assert.Equal(string.Empty, _viewModel.FirstNumberInput);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("12.34")]
        [InlineData("!@#")]
        public void Set_SecondNumber_WithInvalidInput(string invalidInput)
        {
            // Act
            _viewModel.SecondNumberInput = invalidInput;

            // Assert
            Assert.Equal(string.Empty, _viewModel.SecondNumberInput);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("0")]
        [InlineData("99999")]
        public void Set_FirstNumber_WithValidInput(string validInput)
        {
            // Act
            _viewModel.FirstNumberInput = validInput;

            // Assert
            Assert.Equal(validInput, _viewModel.FirstNumberInput);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("0")]
        [InlineData("99999")]
        public void Set_SecondNumber_WithValidInput(string validInput)
        {
            // Act
            _viewModel.SecondNumberInput = validInput;

            // Assert
            Assert.Equal(validInput, _viewModel.SecondNumberInput);
        }

        [Fact]
        public void CanExecuteButton_WhenNotBusy()
        {
            // Arrange
            _viewModel.IsBusy = false;

            // Act & Assert
            Assert.True(_viewModel.AddNumbersCommand.CanExecute(null));
        }

        [Fact]
        public void CanExecuteButton_WhenBusy()
        {
            // Arrange
            _viewModel.IsBusy = true;

            // Act & Assert
            Assert.False(_viewModel.AddNumbersCommand.CanExecute(null));
        }
    }
}
