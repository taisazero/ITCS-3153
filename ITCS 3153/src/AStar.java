
/**
 * 
 * @author Erfan Al-Hossami
 * @date 9/17/2018
 *
 *
 */
import java.util.ArrayList;

public class AStar {

	private Node[][] nodes;

	private ArrayList<Node> openList; // stores discovered nodes
	private ArrayList<Node> closedList; // stores visited nodes
	private ArrayList<Node> path;// stores the path from start to goal

	private Node start;// start node
	private Node goal; // goal node

	private int boundary;
	private boolean pathFound = false; // set to true when a path is found

	// Constructor

	public AStar(Node[][] world, int sRow, int sCol, int gRow, int gCol, int bound) {

		// Initialize vars

		boundary = bound;

		nodes = world;

		openList = new ArrayList<Node>();
		closedList = new ArrayList<Node>();

		start = nodes[sRow][sCol];
		goal = nodes[gRow][gCol];

		// Process Start Node

		start.setParent(null);
		start.setG(0);

		start.setH(calcHeuristic(start));
		start.setF();
		openList.add(start);
		
		// START A*
		braveShine();

	}

	public void braveShine() {

		// From video:

		Node current;
		while (openList.size() != 0) {

			// Step 1: Pop off node with lowest F value
			current = findLowestF();
			openList.remove(current);

			// Step 2: Check if the current node is the goal
			if (current.equals(goal)) {

				// if yes generate path
				generatePath();

				// set global flag to true
				pathFound = true;
			} else {

				// if no, generate neighbors and add the to closed list
				generateNeighbors(current);
				closedList.add(current);
			}

		}
	}

	/*
	 * Generates neighbors and adds to the openlist if the node is valid
	 */
	public void generateNeighbors(Node n) {

		int row = n.getRow();

		int col = n.getCol();

		/*
		 * c-1 c c+1 r-1 -> x x x r -> x * x r+1 -> x x x
		 */
		int[] rows = { row - 1, row, row + 1 };
		int[] cols = { col - 1, col, col + 1 };

		for (int r : rows) {
			for (int c : cols) {
				try {
					if (!(r == row && c == col)) {
						if (isValid(nodes[r][c])) {

							// Calculate G
							int moveCost = n.getG();
							if (r == row || c == col) {
								moveCost += 10;
							} else {
								moveCost += 14;
							}
							if (nodes[r][c].getG() == 0 || nodes[r][c].getG() > 0 && moveCost < nodes[r][c].getG()) {
								nodes[r][c].setG(moveCost);

								// Calculate H
								nodes[r][c].setH(calcHeuristic(nodes[r][c]));

								// Calculate F
								nodes[r][c].setF();
								nodes[r][c].setParent(n);
								if (!openList.contains(nodes[r][c])) {
									openList.add(nodes[r][c]);
								} // end if
							} // end g-cost if
						} // if valid if
					} // end not current node if
				} // end try
				catch (IndexOutOfBoundsException e) {
					continue;
				} // end catch
			} // end col loop
		} // end row loop

	}

	public int calcHeuristic(Node n) {

		int row = n.getRow();
		int col = n.getCol();
		return (Math.abs(goal.getRow() - row) + Math.abs(goal.getCol() - col)) * 10;
	}

	public void generatePath() {

		path = new ArrayList<Node>();
		Node cur = goal;

		while (cur.equals(start) == false) {
			path.add(cur);
			cur = cur.getParent();
		}
		path.add(cur);
	}

	
	/*
	 * 
	 * GETTERZ
	 */
   public ArrayList<Node> getPath() {
        return path;
    }

    public boolean isPathFound() {
        return pathFound;
    }
    
	/*
	 * Util Methods
	 */

	public boolean isPathable(Node n) {

		if (n.getType() == Node.PATHABLE) {
			return true;
		} else {
			return false;
		}
	}

	public boolean withinBounds(Node n) {

		int r = n.getRow();
		int c = n.getCol();

		return r >= 0 && r < boundary && c >= 0 && c < boundary;
	}

	public boolean isValid(Node n) {

		return withinBounds(n) && isPathable(n) && !closedList.contains(n);
	}

	public Node findLowestF() {

		// finds the lowest F node in the openList

		if (openList.size() != 0) {
			Node target = openList.get(0);

			for (Node s : openList) {

				if (s.getF() < target.getF()) {
					target = s;
				}
			}
			return target;
		} else {
			return null;
		}
	}

}
