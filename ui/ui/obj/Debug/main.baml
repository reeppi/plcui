   M S B A M L   `   `   `  ����=  9ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:uiui  5   	   A =libPLC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:libPLC;assembly=libPLClibPLC 5   	       ui.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   "localclr-namespace:ui     5   	   2p$clr-namespace:libPLC;assembly=libPLC     5   	   -   5          � Title$  PLC UI��5	   	   $	��600��6   $
��1024��6%    � WindowState 5
      �� *
WinState+��5      �� 5   
    �� EndPoint$ 0.8,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �    themeColor1 � Color# [  6    � Offset$ 0Q�6@   � 5        themeColor3# [ 6   $
 0.77Q�6@   5      .��5      �� .+�5   
    � -   5          �Name$ frame��5       
 frameW#��[ 6    
 frameH#��[ 6?   $
��Left=�6`     � NavigationUIVisibility$ Hidden=�6�      ui.navButton  5   
      Text$
 AUTO��6   	   Page$
	 auto��6B   

 �� Top$	
 482��6�   $��77��6�   $	��125��6�   $��18(�6�   $
��Bold"�6�      CurPage 6&   �� *	CurPage+   
NavService 6N   �� *NavigationService+ �� ElementName$ frame��6�     5   
   $ 	ASETUKSET��6   $	 settings��6G    �� Left$	 250��6�   $	
 482��6�   $��77��6�   $	��125��6�   $��18(�6�   $
��Bold"�6�    J� RenderTransformOrigin$ 0.125,0.443&�6�    6+   �� *	CurPage+ 6W   �� *NavigationService+$ frame��6�     5   
   $	 PLC��6   $		 plc��6A   $	 375��6�   $	
 482��6�   $��77��6�   $	��103��6�   $��18(�6�   $
��Bold"�6�    6%   �� *	CurPage+ 6L   �� *NavigationService+$ frame��6�   �� 5   
   -   5         ��Name$ checkBox��5      $��On-screen keyboard��6A   $	 847��6^   $	
 528��6p    d� 	IsChecked 6%   �� *Touch+6p   �� 5   
   $	��972��6   $	 450��6   $
 35��60   $��78��6@   $ 0.5,0.5&�6K    K� ZIndex$ -1��6k   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$	 -90Q�6&   T� 5      5       ��5"      �� 5#      $ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5$      � $ 	#FFBFBFBF��6#   $ 0.471Q�65   � 5%      $ 1Q�6#   5&      5(        5)   
   $ MANUAALI��6   $	 manual��6F   $	
 482��6�   $��77��6�   $	��125��6�   $��18(�6�   $
��Bold"�6�   $	 125��6�    6*   �� *	CurPage+ 6T   �� *NavigationService+$ frame��6�   5,      5/      