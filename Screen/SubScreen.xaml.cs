﻿using System.Windows;
using System.Windows.Media;

namespace Goguma.Screen
{
  /// <summary>
  /// Interaction logic for SubScreen.xaml
  /// </summary>
  public partial class SubScreen
  {
    protected SubScreen()
    {
      InitializeComponent();
      BGColor = screen.BGColor;
      FGColor = screen.FGColor;
      screen.IsSubScreen = true;
    }

    /// <summary>
    /// 보조스크린을 새로 생성합니다.
    /// </summary>
    /// <param name="parent">부모 스크린</param>
    /// <param name="title">이름</param>
    /// <param name="size">보조스크린 크기</param>
    public SubScreen(Screen parent, string title, Size size) : this()
    {
      this.Width = size.Width;
      this.Height = size.Height;
      borderBox.Header = title;
      BGColor = parent.BGColor;
      FGColor = parent.FGColor;
      screen.Parent = parent;
      screen.KeySet = parent.KeySet;
      screen.ParentSubScreen = this;
    }
    
    /// <summary>
    /// 배경 색
    /// </summary>
    public Brush BGColor
    {
      get => screen.BGColor;
      set
      {
        screen.BGColor = value;
        Background = value;
      }
    }
    
    /// <summary>
    /// 글자 색
    /// </summary>
    public Brush FGColor
    {
      get => screen.FGColor;
      set
      {
        screen.FGColor = value;
        Foreground = value;
      }
    }

    private void SubScreen_OnLoaded(object sender, RoutedEventArgs e)
    {
      screen.Focus();
    }

    public void SetSize(double width, double height)
    {
      this.Width = width;
      this.Height = height;
    }
  }
}
