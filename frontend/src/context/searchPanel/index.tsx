import { useQuery, UseQueryResult } from '@tanstack/react-query'
import {
  createContext,
  Dispatch,
  FunctionComponent,
  SetStateAction,
  useContext,
  useState,
} from 'react'
import { EntryService } from '../../services/entryService'
import { useDebounceValue } from 'usehooks-ts'
import { GetEntriesResponse } from '../../models/entry/Entry'

interface SearchPanelContextType {
  setSearchValue: Dispatch<SetStateAction<string>>
  searchValue: string
  searchQuery: UseQueryResult<GetEntriesResponse, Error>
  debounceSearchValue: string
}

interface SearchPanelProps {
  children: JSX.Element | JSX.Element[]
}

const SearchPanelContext = createContext<SearchPanelContextType | undefined>(
  undefined
)

const SearchPanelContextProvider: FunctionComponent<SearchPanelProps> = ({
  children,
}) => {
  const [searchValue, setSearchValue] = useState<string>('')

  const [debounceSearchValue] = useDebounceValue(searchValue, 200)

  const searchQuery = useQuery({
    queryKey: ['search', 'entries', debounceSearchValue],
    enabled: !!debounceSearchValue,
    queryFn: async () => await EntryService.getEntries(debounceSearchValue),
  })

  return (
    <SearchPanelContext.Provider
      value={{ searchValue, setSearchValue, searchQuery, debounceSearchValue }}
    >
      {children}
    </SearchPanelContext.Provider>
  )
}

export default SearchPanelContextProvider

export const useSearchPanelContext = () => useContext(SearchPanelContext)
