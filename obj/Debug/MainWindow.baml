   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   E ANorne Beta, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Norne_Beta
Norne_Beta 5   	   =#clr-namespace:Norne_Beta.UIElementsNorne_Beta.UIElements 5	   	      Norne_Beta.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=2.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   Jswf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms  5   	   $localclr-namespace:Norne_Beta  5   	   4
UIElements#clr-namespace:Norne_Beta.UIElements  5	   	     � Title$  Norne��5   	   $��938.463��6   $��1562.33��6(    J� InputBindings 5      �� 5   
   
 �� Key$	 F10��6   6!   .��5      � .+�5   
   � -   5         �Name$ MenuGrid��5      $��25��6!   $	��Top=�6-   .+�5      o� -   5         o�Name$
 menu��5      $��25��6!   $	��Top=�6-    � UseLayoutRounding .  $��Stretch=�6_   .@�5      m� $
��File��6   $��Center=�6)   .@�5      m� -   5         m�Name$ FileNewItem��5      $	��New��64   .@�5      m� -   5        $ NewPorjectItem��5   #   $��New Project��6;   6P   5      m� 5      -   5        $��Add Template To...��5       m� InputGestureText$	 F10��6_   m� 5      -   5        $��Save Template As��5      69   m� 5      -   5        $��Load Template��5      66   5      m� 5      $��Build��6   .@�5      m� -   5        $��Build Template��5      $	 F10��6[   5      5      5      �� 5   
   -	   5         ��Name$ buttonTextBox��5      $��TextBox��6(   $
��Left=�6:   $��
11,837,0,0q�6U   $	��Top=�6i   $��75��6�   6�   �� 5    
   -
   5        $ buttonAddVertical��5       $��VerticalTemplate��6,   $
��Left=�6G   $��
93,876,0,0q�6b   $	��Top=�6v   $��94��6�   6�   �� 5!   
   -   5        $ buttonAddHorizontal��5!      $��Template��6.   $
��Left=�6A   $��
10,802,0,0q�6\   $	��Top=�6p   $��75��6�   $��22��6�   � 5"   
   $��0,24,4,0q�6   	 � ColumnDefinitions	 5#      �� 5$      $
��616* �6#   �� 5%      $��8 �6#   �� 5&      $
��155* �6#   .+�5(      �� -   5        
 ��Name$
 
tabControl��5(      $��	230,0,0,2q�6-    �� SelectedIndex$ 1��6@   .@�5)      �� $��Project��6   .��5*      � ���	#FFE5E5E5.+�5+      � -   5        $ ProjectViewGrid��5+       � ShowGridLines .  .   J� OpacityMask �	#FF8F8383���	#FFC5C5C5 J� 	AllowDrop .  � RowDefinitions 5,      �� 5-   "   	 5/      �� 50   "    �� 	AllowDrop . 63   52      53      54      �� 55      $��Template��6   .��56      � -   5        $ TemplateViewGrid��56   2   ���	#FFE5E5E5.+�57      ]� -   5         ]�Name$ BaseTemplateDockPanel��57   $   ��.  ���	#FFC1BCBC . 6�   58      59      5:      �� 5;      -   5        $ button��5;      $��Connect VIZ��6%   $
��Left=�6;   $��	0,852,0,0q�6V   $	��Top=�6i   $��75��6�   $��22��6�   �� 5<      -   5         ��Name$ label��5<      $��
Properties��6#   $��627,20,-426,0q�68   $	��Top=�6O   $��30��6g    J� RenderTransformOrigin$ 	1.297,0.5&�6s   $��2��6�   �� 5=      -   5        $ button1��5=      $��ComboBox��6&   $
��Left=�69   $��
10,207,0,0q�6T   $	��Top=�6h   $��75��6�   $��22��6�   R� 5>      -   5         R�Name$ treeView��5>      $
��Left=�6)   $	��413��6D   $��
10,330,0,0q�6Q   $	��Top=�6e   $	��202��6}   .@�5?      Q� $��Common Controls��6    Q� 
IsExpanded . .@�5@      Q� -   5        $��	TextPanel��5@   #   $
��Left=�66   $��92��6Q    . 6m   Q� 5A      -   5        $��ComboBox��5A   #   $
��Left=�65   $��92��6P    . 6l   Q� 5B      -   5        $��Table��5B   #   $
��Left=�62   $��92��6M    . 6i   Q� 5C      -   5        $��Button��5C   #   $
��Left=�63   $��92��6N    . 6j   Q� 5D      -   5        $��	DockPanel��5D   #   $
��Left=�66   $��92��6Q    . 6m   5E      5F      1  +Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid  5H      -   5          Name$ _propertyGrid��5H       $��	5,2,0,479q�67      AutoGenerateProperties .  $��2��6i   ���	#FF6B6E746�   �� 5I      -   5         ��Name$ gridSplitter��5I      $��Stretch=�61   $��1��6O   $��0,0,2,0q�6_   �� 5J      -   5         ��Name$ TemplatesListView��5J      $
��Left=�62   $	��293��6M   $��	11,28,0,0q�6Z   $	��Top=�6m   $	��200��6�    �� View 5K      �� 5L       �� ColumnHeaderStringFormat$  ��6   .P�5M      �� $��	Templates��6)   5N      5P      R� 5Q      -   5        $ 	treeView1��5Q      $��2��6*   $	��468��6:   $��
10,410,4,0q�6G   $	��Top=�6[   .@�5R      Q� $��Control Objects��6   .@�5S      Q� $
��HAHA��6#   Q� 5T      $��b��6#   Q� 5U      $��c��6#   Q� 5V      $��d��6#   5W      5X      5Y      5Z      5[      