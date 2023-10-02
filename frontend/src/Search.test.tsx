import {render, screen} from "@testing-library/react";
import userEvent from '@testing-library/user-event'
import {Search} from './Search';
import {APIService} from "./APIService.ts";

describe("test suite", () => {
    test("should have elements", () => {
        render(<Search />);
        expect(screen.getAllByRole('button')).toHaveLength(2);
    });
    
    
    test("mocking", async () =>{
        render(<Search />);
        let button = screen.getByText("Search");
        await userEvent.click(button);
        
        
        APIService.search = vi.fn().mockReturnValue([]);
        
        expect(screen.getByText('No result')).toBeDefined();
        
        
    });
});