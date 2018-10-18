/**
 * 
 * @author Erfan Al-Hossami
 * @date 9/17/2018
 *
 *
 */
import java.util.Scanner;
public class Driver {
	
	public static void main (String [] args) {
		// 15x15 map
		char choice ='y';
		Map m = new Map ();
		Scanner sc = new Scanner (System.in);
		int startX=0,startY=0,goalX=0,goalY=0;
		
		//testing unpathables 
		
		
		  /*String[][] m2 = {{"-", "x", "-", "-", "-", "-"},
                  {"-", "x", "-", "-", "-", "-"},
                  {"-", "x", "-", "x", "-", "-"},
                  {"-", "x", "-", "x", "-", "-"},
                  {"-", "x", "-", "x", "-", "-"},
                  {"-", "x", "-", "-", "-", "-"}};
		  m = new Map(m2);*/
		  
		
		do {
			m.createMap(); // uncomment me when testing the unpathable map
			System.out.println("Raw Map: \n\n");
			System.out.println(m.toString());
		
		do{
		System.out.print("Enter the row coordinate of your starting node: ");
		startX= sc.nextInt();
		System.out.print("Enter the column coordinate of your starting node: ");
		startY= sc.nextInt();
		System.out.print("Enter the row coordinate of your goal node: ");
		goalX=sc.nextInt();
		System.out.print("Enter the column coordinate of your goal node: ");
		goalY=sc.nextInt();
		
		if (! (valid(m,startX,startY) && valid(m,goalX,goalY))){
			System.out.println("Please input a valid goal/start node. It needs to be pathable and within the bounds.");
		}
		
		}while (! (valid(m,startX,startY) && valid(m,goalX,goalY)) ); //end while
		
		m.setElement(startX, startY, "s");
		m.setElement(goalX, goalY, "g");
		System.out.println("Below is the map with labels: "+"\n\n\n");
		System.out.println(m.toString());
		
		
		m.generatePath(startX, startY, goalX, goalY);
		m.displayPath();
		
		 m.updateMap();
         System.out.println("\n\n" + m.pathToString());
         
         System.out.print("Would you like to try again? (y/n) ");
         choice = sc.next().toLowerCase().charAt(0);
         m.resetNodes();
 		 m.resetPath();
 		 m.setElement(startX, startY, "-"); // reset start
 		 
 		 m.setElement(goalX, goalY, "-"); // reset goal
		} while (choice == 'y');
         
		
		
	}
	
	public static boolean valid(Map m,int r, int c){
		return r>=0 && r <15 && c>=0 && c<15 && m.getType(r, c).equals(Map.PATHABLE) ;
	}
}
