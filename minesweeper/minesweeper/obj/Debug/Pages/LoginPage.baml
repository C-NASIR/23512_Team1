   M S B A M L   `   `   `  ����F  BMinesweeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Minesweeper.PagesMinesweeper.Pages  5      !    Minesweeper.Pages.LoginPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Minesweeper.Pages     5        N� Title$  	LoginPage��5	      $��470.52��6   $	��750��6,   .��5
      � ��5   
   �� 5       �� ImageSource$" ../Images/Gridbackground.png��6    j� Stretch$
 Fill=�6D   .+�5   
   �� -   5         ��Name$ txtBoxClumns��5       �� TextAlignment$ Center=�6(   $
��0,13q�6?   
�������$
��Left=�6`   $��46��6{   $��339,100,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $��Columns��6�   $	��Top=�6�   $	��198��6�   $��18(�6�   $��SimSun-ExtB)�6�   $��1��6N  .��6[   � 	Resources 5       �� 
TargetType �� Property	 �� Value��%_   '��       �� 5      " ��6   .�5      ��  �@��CornerRadius$	 10��65   5      5      .��6!   5      �� 5   
   -   5        $ 
txtBoxRows��5      $ Center=�6&   $
��0,13q�6=   
�������$
��Left=�6^   $��46��6y   $��339,175,0,0q�6�   $
 Wrap=�6�   $
��Rows��6�   $	��Top=�6�   $	��198��6�   $��18(�6�   $��SimSun-ExtB)�6�   $��2��6E  .��6R   5      ��%_   '��       �� 5      " ��6   .�5      ��  �@��CornerRadius$	 10��65   5      5      .��6!   5      �� 5   
   -   5        $ txtBoxMines��5      $ Center=�6'   
�������$
��0,13q�6P   $
��Left=�6_   $��46��6z   $��339,250,0,0q�6�   $
 Wrap=�6�   $��Mines��6�   $	��Top=�6�   $	��198��6�   $��18(�6�   $��SimSun-ExtB)�6�   $��3��6I  .��6W   5      ��%_   '��       �� 5      " ��6   .�5      ��  �@��CornerRadius$	 10��65   5       5!      .��6!   5"      �� 5#   
   -   5        
 ��Name$
 txtBlockTitle��5#      
�������$
��Left=�6E   $��
198,10,0,0q�6`   $
��Wrap=�6t    �� Padding$	 0,7q�6�    �� TextAlignment$ Center=�6�   $��Choose Your Grid��6�   $	��Top=�6�   $	��489��6�   $��65��6�   $��36(�6�   $��Segoe WP Black)�6  $��
ExtraBlack"�6  $��	Underline{�67  $��ExtraCondensed&�6S  �� 5$   
   -   5         ��Name$ 	btnSubmit��5$      $��SUBMIT��6$   {�.  
�������
��� � �$
��Left=�6n   $��339,336,0,0q�6�   $	��Top=�6�   $	��198��6�   $��52��6�   $��36(�6�   $��Franklin Gothic Demi)�6�   $��4��6   5%      ��%_   '��       �� 5&      " ��6   .�5'      ��  �@��CornerRadius$	 10��65   5(      5)      5*      �� 5+   
   -   5        $
 tbkColumnError��5+      
���  ��$ Center=�6=   $�� ��6T   $
��Left=�6\   $��273,146,0,0q�6w   $	��Top=�6�   $	��341��6�   $��29��6�   $��Sitka Small)�6�   $��14(�6�   �� 5,   
   -   5        $
 tbkRowError��5,      
���  ��$ Center=�6:   $�� ��6Q   $
��Left=�6Y   $��273,221,0,0q�6t   $	��Top=�6�   $	��341��6�   $��29��6�   $��Sitka Small)�6�   $��14(�6�   �� 5-   
   -   5        $
 tbkMineError��5-      
���  ��$ Center=�6;   $�� ��6R   $
��Left=�6Z   $��273,296,0,0q�6u   $	��Top=�6�   $	��341��6�   $��29��6�   $��Sitka Small)�6�   $��14(�6�   �� 5.   
   -	   5        $
 	textBlock��5.      
���p�$
��Left=�6A   $��	10,75,0,0q�6\   $
��Wrap=�6o   $����Minesweeper has its origins in the earliest mainframe games of the 1960s and 1970s. The earliest ancestor of Minesweeper was Jerimac Ratliff's Cube. The basic gameplay style became a popular segment of the puzzle game genre during the 1980s, with such titles as Mined-Out (Quicksilva, 1983), Yomp (Virgin Interactive, 1983), and Cube. Cube was succeeded by Relentless Logic (or RLogic for short), by Conway, Hong, and Smith, available for MS-DOS as early as 1985; the player took the role of a private in the United States Marine Corps, delivering an important message to the U.S. Command Center. RLogic had greater similarity to Minesweeper than to Cube in concept, but a number of differences exist:��6�   $	��Top=�6H  $	��183��6`  $	��386��6l  $��Global User Interface)�6y  $
��Bold"�6�  �� 5/   
   -
   5        $
 
textBlock1��5/      $
��Left=�6(   $��	10,26,0,0q�6C   $
��Wrap=�6V   $ Center=�6j   
�������$ ��LITTLE HISTORY OF THE GAME��6�   $	��Top=�6�   $	��183��6�   $��44��6�   $��Rockwell)�6�   $��14(�6�   $��	Underline{�6
  50      51      