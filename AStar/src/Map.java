
/**
 * 
 * @author Erfan Al-Hossami
 * @date 9/17/2018
 *
 *
 */
import java.util.ArrayList;
import java.util.*;

public class Map {

	// instance vars

	private String[][] map;
	private int mapSize = 15;
	private String[][] mapPath;

	private Node[][] nodes;

	private ArrayList<Node> nodePath;

	public static String UNPATHABLE = "x";

	public static String PATHABLE = "-";

	/*
	 * Dem Construtorz
	 */
	public Map() {

		map = new String[mapSize][mapSize];
		createMap();
		nodes = new Node[mapSize][mapSize];
		createNodes();
		mapPath = new String[mapSize][mapSize];

	}

	public Map(int size) {

		map = new String[size][size];
		createMap();
		nodes = new Node[size][size];
		createNodes();
		mapPath = new String[size][size];

	}

	public Map(String[][] mapy) {
		map = mapy;
		int tempSize = mapy.length;
		mapSize = tempSize;
		nodes = new Node[tempSize][tempSize];
		createNodes();
		mapPath = new String[tempSize][tempSize];

	}

	public Map(String[][] mapy, String path, String noPath) {
		UNPATHABLE = noPath;
		PATHABLE = path;
		map = mapy;
		int tempSize = mapy.length;
		nodes = new Node[tempSize][tempSize];
		createNodes();
		mapPath = new String[tempSize][tempSize];

	}

	public void createMap() {
		// 10% unpathable
		for (int r = 0; r < map.length; r++) {

			for (int c = 0; c < map[r].length; c++) {
				boolean isPathable = true;
				int rand = (int) (Math.random() * 10);
				if (rand == 0) {
					isPathable = false;
				}
				if (isPathable) {
					map[r][c] = PATHABLE;

				} else {
					map[r][c] = UNPATHABLE;
				}

			}
		}

	}

	public void createNodes() {

		for (int r = 0; r < map.length; r++) {

			for (int c = 0; c < map[r].length; c++) {
				int nodeType = map[r][c].equals(UNPATHABLE) ? Node.UNPATHABLE : Node.PATHABLE;
				nodes[r][c] = new Node(r, c, nodeType);
			}
		}
	}

	public void generatePath(int startR, int startC, int goalR, int goalC) {

		AStar linkStart = new AStar(nodes, startR, startC, goalR, goalC, mapSize);
		if (linkStart.isPathFound() == true) {
			nodePath = linkStart.getPath();
		} else {
			nodePath = null;
		}

	}

	public void displayPath() {

		if (nodePath == null)
			System.out.println("no path found");
		else {
			Collections.reverse(nodePath);
			nodePath.forEach((node) -> System.out.print((node.toString() + " ")));
			Collections.reverse(nodePath);
		}

	}

	/*
	 * Creates numbers for the nodes in the path
	 */
	public void updateMap() {

		resetMapPath();

		if (nodePath != null) {
			int counter = 1;
			for (int i = nodePath.size() - 1; i >= 0; i--) {
				Node n = nodePath.get(i);
				int r = n.getRow();
				int c = n.getCol();
				mapPath[r][c] = "" + counter;
				counter++;

			}
		}

	}

	/*
	 * Reset stuff
	 */

	private void resetMapPath() {
		for (int r = 0; r < map.length; r++) {

			for (int c = 0; c < map[r].length; c++) {

				mapPath[r][c] = map[r][c];

			}

		}
	}

	public void resetNodes() {
		nodes = new Node[mapSize][mapSize];
		createNodes();
	}

	public void resetPath() {
		if (nodePath != null) {
			
			nodePath.clear();
		}

	}

	/*
	 * Map Setters and Getters
	 */

	public String[][] getMap() {
		return map;
	}

	public String[][] getPath() {

		return mapPath;
	}

	public int getMapSize() {
		return mapSize;
	}

	public String getType(int row, int col) {

		return map[row][col];

	}

	public void setElement(int row, int column, String element) {
		map[row][column] = element;
	}

	public String toString() {
		String result = "";

		result += "\t";

		for (int r = 0; r < mapSize; r++) {
			result += r + "\t";
		}

		result += "\n";

		for (int r = 0; r < mapSize; r++) {
			result += r + "\t";
			for (int c = 0; c < mapSize; c++) {

				result += map[r][c] + "\t";

			}
			result += "\n";

		}

		return result;
	}

	public String pathToString() {

		String result = "";

		result += "\t";
		for (int r = 0; r < mapSize; r++) {
			result += r + "\t";
		}
		result += "\n";

		for (int r = 0; r < mapSize; r++) {
			result += r + "\t";
			for (int c = 0; c < mapSize; c++) {
				result += mapPath[r][c] + "\t";
			}
			result += "\n";
		}
		return result;

	}
}
