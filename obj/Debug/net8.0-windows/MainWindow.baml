   M S B A M L   `   `   `  ����  AIAppLauncher    AIAppLauncher.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  AI App Launcher��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�6#   
������  � Topmost$
 True��5
   	   ��5   	   �� */-SystemParameters.MaximizedPrimaryScreenHeight+��5   	   �� *.,SystemParameters.MaximizedPrimaryScreenWidth+ � 	Resources 5         RoundedButtonStyle �� 
TargetType �� Property �� Value	 �� 
TargetType
 ��Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	        �� 5   
   " ��6+   .�5      ��  � �� 5      �� 5      "	 ��6&   .R�5      �� $
 border��6!   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver$
 True��6=   . �5   "   �� $ border��6)    �@J�Opacity$	 0.7Q�6P   �� 5   "    �@�Cursor$
 Hand��6;   5      5      5      5      5      .��5      � -   5         �Name$ MainGrid��5      .+�5!   
   �� -   5         ��Name$ Overlay��5!      ���	#900000006%   �� 5$   
   -   5         ��Name$ AppLauncherPanel��5$      $��Center=�6/   $��Center=�6J   .+�5&      �� -   5         ��Name$ AppIconsControl��5&      ��5'      �� 5(      .R�5)      ��  �� Orientation$ 
Horizontal=�6%   5*      ��5,      �� 5-      .R�5.      �� -   5        $��8q�50   !   #��[  51   !   ��5.   !   �� *DataContext.ButtonSize+ �� RelativeSource ��  �� AncestorType
 Window��5/   !   �� *DataContext.ButtonSize+ �� 
 Window.��54      �� $��Uniform=�6@   $��8q�6R   ��6$   �� *
IconPath+6R   55      56      58      5A      5B      5C      