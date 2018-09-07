import random
import copy

class StateBoard:

    def __init__(self,n):
        self.board=[[0]*n for _ in range(n)]
        self.n=n
        self.generate_rand_grid()


    def set_board(self,board):
        self.board=board

    def add_queen(self,x,y):

        self.board[x][y] = 1


    def generate_rand_grid (self):
        for column in range(0,self.n):
            rand = int(random.random()*8)
            self.add_queen(rand,column)



    def move_queen(self,start_r,start_c,end_row,end_col):
        if self.board [start_r][start_c] == 1:
            self.board [start_r][start_c]= 0
            self.board [end_row][end_col] = 1


    def is_goal(self):
        if self.calc_heuristic() == 0:
            return True

        else:
            return False


    def get_neighbor_states(self):
        states = {}
        for row in range(0,self.n):
            for column in range(0, self.n):
                if self.board[row][column] == 1:
                    for i in range (0,self.n):
                        if i == row:
                            continue
                        new_state = StateBoard(8)
                        new_state.set_board(copy.deepcopy(self.board))
                        new_state.move_queen(row,column,i,column)
                        states[new_state] = new_state.calc_heuristic()
        return states

    def within_bounds(self,num):
        return num<self.n and num>= 0

    def safe_column (self,queen_row,queen_column):
        conflicts = 0
        for column in range(0,self.n):
            if column !=queen_column:
                if self.board[queen_row][column] == 1:
                    conflicts += 1

        return conflicts

    def safe_row(self, queen_row, queen_column):
        conflicts = 0
        for row in range(0, len(self.board)):
            if row != queen_row:
                if self.board[row][queen_column] == 1:
                    conflicts += 1

        return conflicts

    def safe_diagonal_1(self,queen_row,queen_column):
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

    def safe_diagonal_2(self, queen_row, queen_column):
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

    def calc_heuristic(self):
        conflicts = 0
        for row in range (0,self.n):
            for column in range (0,self.n):
                    if self.board[row][column] == 1:
                        conflicts += self.safe_column(row,column) + self.safe_row(row,column) + self.safe_diagonal_1(row,column) + self.safe_diagonal_2(row,column)

        return conflicts/2