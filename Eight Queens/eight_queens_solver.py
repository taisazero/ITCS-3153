
from board import StateBoard

from collections import OrderedDict

restarts = 0
state_changes = 0
SIZE = 8


current_state = StateBoard(SIZE)

while current_state.is_goal()==False:
    h = current_state.calc_heuristic()

    print("Current h: "+ str(h))
    print("Current State: ")
    current_state.display_board()

    neighbor_states= current_state.get_neighbor_states()
    num_lower_neighbors=0
    neighbor_states_sorted = OrderedDict(sorted(neighbor_states.items(), key= lambda x:x[1]))
    #neighbor_states_sorted_list = sorted((value,key) for (key,value) in neighbor_states.items())
    lowest_state = list(neighbor_states_sorted.items())[0][0]
    for state in neighbor_states_sorted.keys():

        if neighbor_states_sorted[state] < h:
            num_lower_neighbors += 1

    print('Neighbors found with lower h: ' + str(num_lower_neighbors))

    if num_lower_neighbors == 0:
        print('RESTART')
        current_state = StateBoard(SIZE)
        restarts +=1

    else:
        print('Setting new current state\n')
        current_state = lowest_state
        state_changes += 1



current_state.display_board()

print('SOLUTION FOUND!')

print('State Changes: '+ str(state_changes))

print('Restarts: '+ str(restarts))


