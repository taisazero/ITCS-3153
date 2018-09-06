class Board:

    def __init__(self,n):
        self.board=[[0]*n for _ in range(n)]
        self.n=n


    def add_queen(self,x,y):

        self.board[x][y] = 1

    def within_bounds (self,num):
        return num<self.n and num>=0

    def is_safe_column (self,queen_row,queen_column):
        conflicts = 0
        for column in range(0,len(self.board[0])):
            if column !=queen_column:
                if self.board[queen_row][column] == 1:
                    conflicts += 1

        return conflicts

    def is_safe_row(self, queen_row, queen_column):
        conflicts = 0
        for row in range(0, len(self.board)):
            if row != queen_row:
                if self.board[row][queen_column] == 1:
                    conflicts += 1

        return conflicts

    def is_safe_diagonal_1(self,queen_row,queen_column):
        temp_x  = queen_row
        temp_y = queen_column
        while temp_x != len(self.board)-1 and temp_y != len(self.board[0])-1:
            temp_x +=1
            temp_y +=1
        conflicts = 0
        while temp_x >=0 and temp_y>=0:
            if temp_x !=queen_row and temp_y!=queen_column:
                if self.board[temp_x][temp_y] == 1:
                    conflicts += 1
            temp_x -= 1
            temp_y -= 1

        return conflicts

    def is_safe_diagonal_2(self, queen_row, queen_column):
        conflicts=0
        temp_x = queen_row
        temp_y = queen_column
        while temp_x != len(self.board)-1 and temp_y != len(self.board[0])-1 and self.within_bounds(temp_x) and self.within_bounds(temp_y):
            temp_x += 1
            temp_y -= 1

        while temp_x >= 0 and temp_y >= 0 and temp_y<len(self.board[0]) and self.within_bounds(temp_y) and self.within_bounds(temp_x):
            if temp_x != queen_row and temp_y != queen_column:
                if self.board[temp_x][temp_y] == 1:
                    conflicts+=1
            temp_x -= 1
            temp_y += 1

        return conflicts


    def display_board(self):
        for row in range(0,len(self.board)):
            for column in range (0,len(self.board[0])):
                print(str(self.board[row][column])+' ',end="", flush=True)

            print()

    def is_safe(self,queen_x,queen_y):
        return self.is_safe_column(queen_x,queen_y) and self.is_safe_row(queen_x,queen_y) and self.is_safe_diagonal_1(queen_x,queen_y) and self.is_safe_diagonal_2(queen_x,queen_y)
