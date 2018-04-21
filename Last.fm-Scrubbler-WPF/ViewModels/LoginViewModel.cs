﻿using Caliburn.Micro;
using IF.Lastfm.Core.Api;
using Scrubbler.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scrubbler.ViewModels
{
  /// <summary>
  /// ViewModel for the <see cref="Views.LoginView"/>
  /// </summary>
  class LoginViewModel : Screen
  {
    #region Properties

    /// <summary>
    /// Gets if certain controls should be enabled.
    /// </summary>
    public bool EnableControls
    {
      get { return _enableControls; }
      private set
      {
        _enableControls = value;
        NotifyOfPropertyChange(() => EnableControls);
      }
    }
    private bool _enableControls;

    #endregion Properties

    #region Member

    /// <summary>
    /// Last.fm authentication object.
    /// </summary>
    private ILastAuth _lastAuth;

    /// <summary>
    /// Service for showing MessageBoxes.
    /// </summary>
    private IMessageBoxService _messageBoxService;

    #endregion Member

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="lastAuth">Last.fm authentication object.</param>
    /// <param name="messageBoxService">Service for showing MessageBoxes.</param>
    public LoginViewModel(ILastAuth lastAuth, IMessageBoxService messageBoxService)
    {
      _lastAuth = lastAuth;
      _messageBoxService = messageBoxService;
      EnableControls = true;
    }

    /// <summary>
    /// Tries to log the user in with the given credentials.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The <see cref="PasswordBox"/> containing the password.</param>
    public async Task Login(string username, PasswordBox password)
    {
      EnableControls = false;

      try
      {
        var response = await _lastAuth.GetSessionTokenAsync(username, password.Password);

        if (response.Success && _lastAuth.Authenticated)
        {
          _messageBoxService.ShowDialog("Successfully logged in and authenticated!");
          TryClose(true);
        }
        else
          _messageBoxService.ShowDialog("Failed to log in or authenticate!");
      }
      catch (Exception ex)
      {
        _messageBoxService.ShowDialog("Fatal error while trying to log in: " + ex.Message);
      }
      finally
      {
        EnableControls = true;
      }
    }

    /// <summary>
    /// Logs the user in if the enter key is pressed.
    /// </summary>
    /// <param name="e">EventArgs containing the pressed key.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The <see cref="PasswordBox"/> containing the password.</param>
    public async void ButtonPressed(KeyEventArgs e, string username, PasswordBox password)
    {
      if (e.Key == Key.Enter)
        await Login(username, password);
    }
  }
}